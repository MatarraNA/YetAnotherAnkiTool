using CSCore.Codecs;
using CSCore.Codecs.WAV;
using CSCore.SoundOut;
using System;
using System.Drawing.Imaging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using YetAnotherAnkiTool.Core;
using YetAnotherAnkiTool.Core.API;
using YetAnotherAnkiTool.Core.Config;
using YetAnotherAnkiTool.Core.Control;
using YetAnotherAnkiTool.Core.JSON;

// PUBLISH COMMAND
// dotnet publish -c Release -r win-x64 --self-contained false
namespace YetAnotherAnkiTool
{
    public partial class MainForm : Form
    {
        // Playbutton Variables
        private static readonly string PAUSE_UNICODE = "⏸";
        private static readonly string PLAY_UNICODE = "▶";
        private static bool IsPlaying = false;

        // trackbar variables
        private static bool _suppressTrackBarEvents = false;
        private static bool _isDraggingSeekBar = false;
        private static System.Windows.Forms.Timer? _playbackTimer;
        private static TrackBarOverlay? _trackOverlayPanel;

        // anki variables
        private static System.Windows.Forms.Timer? _ankiPollTimer;
        private static long _lastAnkiId = 0;
        private static readonly Color RED_ANKI_COLOR = Color.FromArgb(240, 192, 192);
        private static readonly Color GREEN_ANKI_COLOR = Color.FromArgb(192, 240, 192);

        // status variables
        private static bool IsCapturing = false;

        // SCREENSHOTTING
        private static System.Windows.Forms.Timer? _screenshotTimer;
        private static PictureBox? _selectedPictureBox = null;
        private static string? _selectedPicturePath = null;

        public double CurrentStartOffset
        {
            get
            {
                try
                {
                    return double.Parse(startOffsetTxtBox.Text);
                }
                catch { }
                return 0d;
            }
        }
        public double CurrentEndOffset
        {
            get
            {
                try
                {
                    return double.Parse(endOffsetTxtBox.Text);
                }
                catch { }
                return 0d;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Enter(object sender, EventArgs e)
        {
            InitializeRecording();
            InitializeOverlay();
            InitializeAnkiPolling();
            InitializeScreenshotRecording();
        }

        private void playAudioBtn_Click(object sender, EventArgs e)
        {
            if (IsCapturing) return; // takes priority over everything

            if (IsPlaying)
            {
                StopPlayback();
                return;
            }

            StopPlayback(); // ensures clean state
            StartPlayback(CurrentStartOffset, CurrentEndOffset);
        }

        private void StartPlayback(double start, double end, float volume = 1)
        {
            if (IsPlaying) return;
            AudioAPI.PlayAudio(start, end, volume);
            playAudioBtn.Text = PAUSE_UNICODE;
            IsPlaying = true;

            Task.Delay(100).ContinueWith(_ =>
            {
                Invoke(() =>
                {
                    var source = AudioAPI.PlaybackSource;
                    var device = AudioAPI.OutputDevice;

                    if (source == null || device == null)
                    {
                        StopPlayback();
                        return;
                    }

                    audioSeekTrackBar.Minimum = 0;
                    audioSeekTrackBar.Maximum = (int)(source.Length / (source.WaveFormat.BytesPerSecond / 4)); // 0.25s steps

                    _playbackTimer = new System.Windows.Forms.Timer { Interval = 50 };
                    _playbackTimer.Tick += (s, args) => UpdatePlaybackUI();
                    _playbackTimer.Start();

                    UpdatePlaybackUI(); // immediate sync
                });
            });
        }
        private void StopPlayback()
        {
            _playbackTimer?.Stop();
            _playbackTimer?.Dispose();
            _playbackTimer = null;

            AudioAPI.StopAudio();
            IsPlaying = false;
            playAudioBtn.Text = PLAY_UNICODE;
        }


        private void InitializeRecording()
        {
            SetCaptureActive(true);
        }
        private void InitializeOverlay()
        {
            // handle drawing the overlay for start / end offset lines
            _trackOverlayPanel = new TrackBarOverlay
            {
                Location = audioSeekTrackBar.Location,
                Size = audioSeekTrackBar.Size,
                GetStartValue = () => (int)(CurrentStartOffset / 0.25),
                GetEndValue = () => (int)(CurrentEndOffset / 0.25),
                BoundTrackBar = audioSeekTrackBar
            };
            Controls.Add(_trackOverlayPanel);
            _trackOverlayPanel.BringToFront();

            var overlayTimer = new System.Windows.Forms.Timer();
            overlayTimer.Interval = 50;
            overlayTimer.Tick += (x, y) =>
            {
                // draw start and end offset lines
                _trackOverlayPanel.Invalidate();
            };
            overlayTimer.Start();
        }
        private void InitializeAnkiPolling()
        {
            _ankiPollTimer = new();
            _ankiPollTimer.Interval = 500;
            _ankiPollTimer.Tick += _ankiPollTimer_Tick;
            _ankiPollTimer.Start();
            _ankiPollTimer_Tick(null, null!);
        }
        private void InitializeScreenshotRecording()
        {
            _screenshotTimer = new();
            _screenshotTimer.Interval = 1000;
            _screenshotTimer.Tick += _screenshotTimer_Tick;
            _screenshotTimer.Start();
            _screenshotTimer_Tick(null, null!);
        }

        private void UpdatePlaybackUI()
        {
            var source = AudioAPI.PlaybackSource;
            var device = AudioAPI.OutputDevice;

            if (source == null || device == null || AudioAPI.IsPlaybackFinished)
            {
                StopPlayback();
                return;
            }

            _suppressTrackBarEvents = true;

            // update bar position
            var quarterSeconds = source.Position / (source.WaveFormat.BytesPerSecond / 4);
            audioSeekTrackBar.Value = Math.Min(audioSeekTrackBar.Maximum, (int)quarterSeconds);

            // update track label
            double seconds = audioSeekTrackBar.Value * 0.25;
            trackPosLabel.Text = $"{seconds:0.00}";
            trackPosLabel.Invalidate();

            // also refresh overlay here
            _trackOverlayPanel?.Invalidate();

            _suppressTrackBarEvents = false;
        }

        private void audioSeekTrackBar_ValueChanged()
        {
            if (IsCapturing) return;
            if (_suppressTrackBarEvents) return;

            var source = AudioAPI.PlaybackSource;
            if (source != null && source.CanSeek && AudioAPI.OutputDevice?.PlaybackState == PlaybackState.Playing)
            {
                long bytePos = (long)(audioSeekTrackBar.Value * (source.WaveFormat.BytesPerSecond / 4.0));
                source.Position = bytePos;
            }
        }

        private void audioSeekTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            _isDraggingSeekBar = true;
            UpdateOffsetFromMouse(e);
        }

        private void UpdateOffsetFromMouse(MouseEventArgs e)
        {
            int newValue = (int)Math.Round((double)(audioSeekTrackBar.Maximum - audioSeekTrackBar.Minimum) * e.X / audioSeekTrackBar.Width);
            newValue = Math.Max(audioSeekTrackBar.Minimum, Math.Min(audioSeekTrackBar.Maximum, newValue));
            audioSeekTrackBar.Value = newValue;

            double seconds = newValue * 0.25;
            if (e.Button == MouseButtons.Left)
            {
                startOffsetTxtBox.Text = $"{seconds:0.00}";
                if (CurrentEndOffset < seconds) endOffsetTxtBox.Text = $"{seconds:0.00}";
            }
            else if (e.Button == MouseButtons.Right)
            {
                endOffsetTxtBox.Text = $"{seconds:0.00}";
                if (CurrentStartOffset > seconds) startOffsetTxtBox.Text = $"{seconds:0.00}";
            }
        }

        private void audioSeekTrackBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraggingSeekBar)
                UpdateOffsetFromMouse(e);
        }

        private void audioSeekTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            _isDraggingSeekBar = false;
        }

        private async void _ankiPollTimer_Tick(object? sender, EventArgs e)
        {
            await RefreshAnkiLabelsFromLatestCard();
        }

        private void _screenshotTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsCapturing) return;
            ScreenshotAPI.CaptureActiveWindow();
        }

        private async Task RefreshAnkiLabelsFromLatestCard()
        {
            try
            {
                long? noteId = await GetLatestNoteIdAsync();
                if (noteId == null)
                {
                    // update label to inform user that ANKI is NOT connected
                    ankiNoteIdLabel.Text = "Anki not Connected. Retrying...";
                    ankiNoteIdLabel.Invalidate();
                    return;
                }
                else if( !Directory.Exists(Config.Configuration.AnkiMediaFolderPath) )
                {
                    // update label to inform user that MEDIA PATH is INVALID
                    ankiNoteIdLabel.Text = "Anki Media folder path is invalid...";
                    ankiNoteIdLabel.Invalidate();
                    return;
                }
                else if (noteId == 0) // this id is found, if getnotes returns an empty array. So it is connected, but no notes were made yet
                {
                    ankiNoteIdLabel.Text = "Anki Connected!";
                    ankiNoteIdLabel.Invalidate();
                    return;
                }
                else if (ankiNoteIdLabel.Text != noteId.Value.ToString())
                {
                    ankiNoteIdLabel.Text = noteId.Value.ToString();
                    ankiNoteIdLabel.Invalidate();
                }

                // does it already have an img?
                string? pictureLoc = await GetCardPictureAsync(noteId.Value);
                if (!string.IsNullOrWhiteSpace(pictureLoc))
                {
                    // it already has an image
                    ankiNoteIdLabel.ForeColor = GREEN_ANKI_COLOR;
                    ankiNoteIdLabel.Invalidate();
                    return;
                }

                if (_lastAnkiId == noteId.Value) return;

                // a NEW card has been found
                _lastAnkiId = noteId.Value;

                // found a valid note, and it is the latest one. It does NOT have an image
                ankiNoteIdLabel.Text = $"{_lastAnkiId}";
                ankiNoteIdLabel.ForeColor = RED_ANKI_COLOR;
                ankiNoteIdLabel.Invalidate();

                // now get text from that card
                string? cardName = await GetCardNameAsync(_lastAnkiId);
                ankiWordLabel.Text = cardName;
                ankiWordLabel.Invalidate();

                // disable capture until this card is dealt with
                SetCaptureActive(false);

                // load in screenshot images
                PopulateScreenshotImages(60); // cap it, but it should never hit cap since capture deletes oldest images

                // play audio for 1 frame, just to load it into memory
                StartPlayback(0, 0.15, 0f);
            }
            catch { }
        }

        public void SetCaptureActive(bool newState)
        {
            if (newState)
            {
                // ensure playback is disabled
                StopPlayback();

                // enable Audio / Screenshot capture
                AudioAPI.StartCapture();

                // set status label
                statusLabel.Text = "Status: Capturing ON";
                statusLabel.Invalidate();

                IsCapturing = true;
            }
            else
            {
                // disable audio / screenshot capture
                AudioAPI.StopCapture();

                // set status label
                statusLabel.Text = "Status: Capturing PAUSED";
                statusLabel.Invalidate();

                IsCapturing = false;
            }
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        private void menuCloseBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void menuMinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ClearScreenshotPanel()
        {
            _selectedPictureBox = null;
            _selectedPicturePath = null;

            imageFlowPanel.Controls.Clear();
        }

        private void PopulateScreenshotImages(int maxImages = 10)
        {
            ClearScreenshotPanel();

            string folderPath = Path.Combine(Environment.CurrentDirectory, "ScreenshotOut");
            var files = Directory.GetFiles(folderPath, "*.jpg")
                                 .Concat(Directory.GetFiles(folderPath, "*.png"))
                                 .OrderByDescending(File.GetLastWriteTime)
                                 .Take(maxImages);

            foreach (var file in files.Reverse()) // newest on right
            {
                var pic = new PictureBox 
                {
                    Image = LoadImageUnlocked(file),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 180,
                    Height = 180,
                    Margin = new Padding(3, 0, 3, 0),
                    Tag = file,
                    Cursor = Cursors.Hand
                };

                pic.Click += Image_Click!;
                pic.DoubleClick += Pic_DoubleClick;
                imageFlowPanel.Controls.Add(pic);
            }

            imageFlowPanel.AutoScrollPosition = new Point(imageFlowPanel.HorizontalScroll.Maximum, 0);
        }

        private void Pic_DoubleClick(object? sender, EventArgs e)
        {
            if (sender is PictureBox pic && pic.Tag is string filePath && File.Exists(filePath))
            {
                using var image = LoadImageUnlocked(filePath);
                var screen = Screen.FromControl(this).WorkingArea;

                // Calculate max allowed size (80% of screen)
                int maxWidth = (int)(screen.Width * 0.8);
                int maxHeight = (int)(screen.Height * 0.8);

                // Scale image down if needed
                int imgWidth = image.Width;
                int imgHeight = image.Height;

                double scale = Math.Min(1.0, Math.Min((double)maxWidth / imgWidth, (double)maxHeight / imgHeight));
                int finalWidth = (int)(imgWidth * scale);
                int finalHeight = (int)(imgHeight * scale);

                var fullSizeForm = new Form
                {
                    Text = Path.GetFileName(filePath),
                    StartPosition = FormStartPosition.CenterParent,
                    BackColor = Color.Black,
                    ClientSize = new Size(finalWidth, finalHeight),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                var fullImage = new PictureBox
                {
                    Image = new Bitmap(image), // clone to avoid disposal issues
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill
                };

                fullSizeForm.KeyDown += (x, y) => 
                {
                    if (y.KeyCode == Keys.Escape) fullSizeForm.Close();
                };
                fullSizeForm.Controls.Add(fullImage);
                fullSizeForm.ShowDialog();
            }
        }

        private void Image_Click(object sender, EventArgs e)
        {
            if (_selectedPictureBox != null)
            {
                // Reset previous selection
                _selectedPictureBox.Padding = new Padding(0);
                _selectedPictureBox.BackColor = imageFlowPanel.BackColor;
            }

            _selectedPictureBox = sender as PictureBox;

            if (_selectedPictureBox != null)
            {
                // Apply highlight
                _selectedPictureBox.Padding = new Padding(2);
                _selectedPictureBox.BackColor = Color.DodgerBlue; // or any highlight color
            }

            _selectedPicturePath = _selectedPictureBox!.Tag as string;
        }

        private Image LoadImageUnlocked(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            return Image.FromStream(ms);
        }

        private async void sendToAnkiBtn_Click(object sender, EventArgs e)
        {
            if (IsCapturing) return;

            // ensure image is selected
            if( string.IsNullOrWhiteSpace(_selectedPicturePath) || !File.Exists(_selectedPicturePath ) )
            {
                MessageBox.Show($"Error: Selected Image: {_selectedPicturePath} - Does not exist");
                return;
            }

            // copy file to media folder, then send to anki
            string screnshotFileName = $"sentToAnki_" + DateTime.Now.Ticks + ".jpg";
            string screenshotAnkiFullPath = Path.Combine(Config.Configuration.AnkiMediaFolderPath, screnshotFileName);
            using var original = Image.FromFile(_selectedPicturePath); // load in original image
            int overrideWidth = Config.Configuration.AnkiImgWidthOverride;
            int overrideHeight = Config.Configuration.AnkiImgHeightOverride;
            Image finalImage = original;
            if (overrideWidth > 0 && overrideHeight > 0) // if overrides exist, resize it
            {
                var resized = new Bitmap(overrideWidth, overrideHeight);
                using var gfx = Graphics.FromImage(resized);
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(original, 0, 0, overrideWidth, overrideHeight);
                finalImage = resized;
            }
            finalImage.Save(screenshotAnkiFullPath, ImageFormat.Jpeg);
            if (!ReferenceEquals(finalImage, original))
                finalImage.Dispose();
            bool worked = await SetCardPictureAsync(_lastAnkiId, screnshotFileName);
            if (!worked)
                MessageBox.Show("Error: Failed to set picture on latest anki card. Check configuration settings.");


            // Sent selected audio to anki
            string soundFileName = $"sentToAnki_" + DateTime.Now.Ticks + ".mp3";
            string soundFileFullPath = Path.Combine(Config.Configuration.AnkiMediaFolderPath, soundFileName);
            AudioAPI.SaveAudioSegmentToFile(soundFileFullPath, CurrentStartOffset, CurrentEndOffset, Config.Configuration.AnkiAudioOutputGain);
            worked = await SetCardAudioAsync(_lastAnkiId, soundFileName);
            if (!worked) MessageBox.Show("Error: Failed to set audio on latest anki card. Check configuration settings.");

            // Clear out images on form to make it visually clearer
            ClearScreenshotPanel();

            // re-enable capture again
            SetCaptureActive(true);
        }

        public async Task<long?> GetLatestNoteIdAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(250);

                var json = "{\"action\":\"findNotes\",\"version\":6,\"params\":{\"query\":\"added:1\"}}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"http://{Config.Configuration.AnkiAddress}:{Config.Configuration.AnkiPort}", content).ConfigureAwait(false);
                var raw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<FindNotesResponse>().ConfigureAwait(false);
                if (result?.Result == null) return null;
                if (result.Result.Count < 1) return 0;

                return result.Result.Max(); // newest note ID
            }
            catch { }
            return null;
        }

        public async Task<string?> GetCardNameAsync(long noteId)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(250);

                var requestJson = $@"
                {{
                    ""action"": ""notesInfo"",
                    ""version"": 6,
                    ""params"": {{
                        ""notes"": [{noteId}]
                    }}
                }}";

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{Config.Configuration.AnkiAddress}:{Config.Configuration.AnkiPort}", content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode) return null;

                var rawJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                using var doc = JsonDocument.Parse(rawJson);

                var fields = doc.RootElement.GetProperty("result")[0].GetProperty("fields");
                if (fields.TryGetProperty(Config.Configuration.AnkiWordFieldName, out var field))
                {
                    return field.GetProperty("value").GetString();
                }
            }
            catch { }

            return null;
        }

        public async Task<string?> GetCardPictureAsync(long noteId)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(250);

                var requestJson = $@"
                {{
                    ""action"": ""notesInfo"",
                    ""version"": 6,
                    ""params"": {{
                        ""notes"": [{noteId}]
                    }}
                }}";

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{Config.Configuration.AnkiAddress}:{Config.Configuration.AnkiPort}", content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode) return null;

                var rawJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                using var doc = JsonDocument.Parse(rawJson);

                var fields = doc.RootElement.GetProperty("result")[0].GetProperty("fields");
                if (fields.TryGetProperty(Config.Configuration.AnkiImgFieldName, out var field))
                {
                    return field.GetProperty("value").GetString();
                }
            }
            catch { }

            return null;
        }

        public async Task<bool> SetCardPictureAsync(long noteId, string mediaFileName)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(250);

                var imgTag = $"<img src={mediaFileName}>";

                var requestJson = $@"
                {{
                    ""action"": ""updateNoteFields"",
                    ""version"": 6,
                    ""params"": {{
                        ""note"": {{
                            ""id"": {noteId},
                            ""fields"": {{
                                ""{Config.Configuration.AnkiImgFieldName}"": ""{imgTag}""
                            }}
                        }}
                    }}
                }}";

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{Config.Configuration.AnkiAddress}:{Config.Configuration.AnkiPort}", content).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetCardAudioAsync(long noteId, string mediaFileName)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(250);

                var soundTag = $"[sound:{mediaFileName}]";

                var requestJson = $@"
                {{
                    ""action"": ""updateNoteFields"",
                    ""version"": 6,
                    ""params"": {{
                        ""note"": {{
                            ""id"": {noteId},
                            ""fields"": {{
                                ""{Config.Configuration.AnkiSoundFieldName}"": ""{soundTag}""
                            }}
                        }}
                    }}
                }}";

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://{Config.Configuration.AnkiAddress}:{Config.Configuration.AnkiPort}", content).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}