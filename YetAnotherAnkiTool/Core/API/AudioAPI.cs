using CSCore;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using NAudio.Lame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace YetAnotherAnkiTool.Core.API
{
    public static class AudioAPI
    {
        private static WasapiLoopbackCapture? _audioLoopbackCapture;
        private static WaveWriter? _waveWriter;
        private static WasapiOut? _outputDevice;
        private static IWaveSource? _playbackSource;


        private static readonly long _maxFileSize = 10 * 1024 * 1024; // 10 MB
        private static readonly int _maxFiles = 10;
        private static int _fileIndex = 0;

        private static readonly string OutputDirectory = Path.Combine(Environment.CurrentDirectory, "AudioOut");

        public static IWaveSource? PlaybackSource => _playbackSource;
        public static WasapiOut? OutputDevice => _outputDevice;
        private static string CurrentFilePath => Path.Combine(OutputDirectory, $"audio_{_fileIndex}.wav");

        static AudioAPI()
        {
            Directory.CreateDirectory(OutputDirectory);

            _audioLoopbackCapture = new WasapiLoopbackCapture();
            _audioLoopbackCapture.Initialize();

            // ensure any corrupted audios are gone
            DeleteCorruptedFiles();

            // Resume from last file index
            var lastFile = Directory.GetFiles(OutputDirectory, "audio_*.wav")
            .OrderBy(f =>
            {
                var name = Path.GetFileNameWithoutExtension(f);
                var digits = new string(name.Where(char.IsDigit).ToArray());
                return int.TryParse(digits, out int num) ? num : 0;
            })
            .LastOrDefault();


            if (lastFile != null)
            {
                var name = Path.GetFileNameWithoutExtension(lastFile);
                var digits = new string(name.Where(char.IsDigit).ToArray());
                int.TryParse(digits, out _fileIndex);
            }

            _audioLoopbackCapture.DataAvailable += (s, e) =>
            {
                _waveWriter?.Write(e.Data, e.Offset, e.ByteCount);
                CheckFileSizeAndRotate();
            };
        }

        private static void DeleteCorruptedFiles()
        {
            // check for corrupted files
            var files = Directory.GetFiles(OutputDirectory, "audio_*.wav")
                     .OrderBy(f => f)
                     .ToList();

            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Length < 10_240)
                        throw new InvalidDataException("File is too small (<10KB).");

                    using var reader = new WaveFileReader(file);

                    // Force header read and basic stream validation
                    var buffer = new byte[reader.WaveFormat.BytesPerSecond];
                    int read = reader.Read(buffer, 0, buffer.Length);

                    if (read <= 0 || reader.Length <= 0)
                        throw new InvalidDataException("File has no readable audio data.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Deleting corrupted file: {file} — {ex.Message}");
                    try { File.Delete(file); } catch { /* ignore */ }
                }
            }
        }

        public static void StartCapture()
        {
            if (_audioLoopbackCapture?.RecordingState != RecordingState.Recording)
            {
                _fileIndex++; // Always start a new file
                var newPath = CurrentFilePath;
                _waveWriter = new WaveWriter(newPath, _audioLoopbackCapture?.WaveFormat);

                _audioLoopbackCapture?.Start();
            }
        }


        public static void StopCapture()
        {
            if (_audioLoopbackCapture?.RecordingState == RecordingState.Recording)
            {
                _audioLoopbackCapture?.Stop();
            }

            _waveWriter?.Dispose();
            _waveWriter = null;
        }

        private static void CheckFileSizeAndRotate()
        {
            var fileInfo = new FileInfo(CurrentFilePath);
            if (fileInfo.Length >= _maxFileSize)
            {
                _waveWriter?.Dispose();
                _waveWriter = null;

                _fileIndex++;
                var newPath = CurrentFilePath;
                _waveWriter = new WaveWriter(newPath, _audioLoopbackCapture?.WaveFormat);

                EnforceMaxFileLimit();
            }
        }

        private static void EnforceMaxFileLimit()
        {
            var files = Directory.GetFiles(OutputDirectory, "audio_*.wav")
                                 .OrderBy(f => f)
                                 .ToList();

            while (files.Count > _maxFiles)
            {
                File.Delete(files.First());
                files.RemoveAt(0);
            }
        }

        public static void PlayAudio(double? startSeconds = null, double? endSeconds = null, float gain = 1.0f, bool forceMono = true)
        {
            StopAudio();

            var sources = GetAudioSources();
            if (sources.Count == 0) return;

            var fullSource = new ChainedWaveSource(sources);
            var sampleSource = fullSource.ToSampleSource();

            // Apply gain
            var volumeSource = new VolumeSource(sampleSource) { Volume = gain };

            // Optional mono conversion
            var monoSource = forceMono ? volumeSource.ToMono() : volumeSource;

            // Convert to 16-bit PCM
            var waveSource = monoSource.ToWaveSource(16);

            var waveFormat = waveSource.WaveFormat;
            int bytesPerSecond = waveFormat.BytesPerSecond;
            int blockAlign = waveFormat.BlockAlign;

            double totalSeconds = (double)waveSource.Length / bytesPerSecond;
            double start = Math.Max(0, startSeconds.GetValueOrDefault(0));
            double end = Math.Min(endSeconds.GetValueOrDefault(totalSeconds), totalSeconds);
            double duration = Math.Max(0, end - start);

            long startBytes = (long)(start * bytesPerSecond);
            long endBytes = (long)(end * bytesPerSecond);

            // Align to block size
            startBytes -= startBytes % blockAlign;
            endBytes -= endBytes % blockAlign;

            // Skip to start
            waveSource.Position = startBytes;

            _playbackSource = waveSource;
            _outputDevice = new WasapiOut();
            _outputDevice.Initialize(_playbackSource);
            _outputDevice.Volume = 1.0f; // gain is already applied
            _outputDevice.Play();

            // Monitor playback and stop at endByte
            Task.Run(() =>
            {
                while (_outputDevice?.PlaybackState == PlaybackState.Playing)
                {
                    if (_playbackSource?.Position >= endBytes)
                    {
                        StopAudio();
                        break;
                    }
                    Thread.Sleep(10);
                }
            });
        }

        //public static void PlayAudio(double? startSeconds = null, double? endSeconds = null, float? volume = null)
        //{
        //    StopAudio();

        //    var sources = GetAudioSources();
        //    if (sources.Count == 0) return;

        //    var fullSource = new ChainedWaveSource(sources);
        //    var waveFormat = fullSource.WaveFormat;
        //    long totalBytes = fullSource.Length;
        //    double totalSeconds = (double)totalBytes / waveFormat.BytesPerSecond;

        //    // Calculate byte offsets
        //    long startByte = 0;
        //    long endByte = totalBytes;

        //    if (startSeconds.HasValue && startSeconds.Value >= 0 && startSeconds.Value < totalSeconds)
        //        startByte = (long)(startSeconds.Value * waveFormat.BytesPerSecond);

        //    if (endSeconds.HasValue && endSeconds.Value > startSeconds.GetValueOrDefault() && endSeconds.Value <= totalSeconds)
        //        endByte = (long)(endSeconds.Value * waveFormat.BytesPerSecond);

        //    if (startByte >= totalBytes || endByte <= startByte)
        //    {
        //        startByte = 0;
        //        endByte = totalBytes;
        //    }

        //    fullSource.Position = startByte;

        //    _playbackSource = fullSource;
        //    _outputDevice = new WasapiOut();
        //    _outputDevice.Initialize(_playbackSource);
        //    _outputDevice.Volume = volume != null ? volume.Value : 1.0f;
        //    _outputDevice.Play();

        //    // Monitor playback and stop at endByte
        //    Task.Run(() =>
        //    {
        //        while (_outputDevice?.PlaybackState == PlaybackState.Playing)
        //        {
        //            if (_playbackSource?.Position >= endByte)
        //            {
        //                StopAudio();
        //                break;
        //            }
        //            Thread.Sleep(10); // adjust for responsiveness
        //        }
        //    });
        //}

        public static void SaveAudioSegmentToFile(string outputPath, double? startSeconds = null, double? endSeconds = null, float gain = 1.0f)
        {
            var sources = GetAudioSources();
            if (sources.Count == 0) return;

            var fullSource = new ChainedWaveSource(sources);
            var sampleSource = fullSource.ToSampleSource();

            // Apply gain
            var volumeSource = new VolumeSource(sampleSource) { Volume = gain };

            // Force mono
            var monoSource = volumeSource.ToMono();

            // Convert to 16-bit PCM
            var waveSource = monoSource.ToWaveSource(16);

            // Calculate sample-level trimming
            var waveFormat = waveSource.WaveFormat;
            int bytesPerSecond = waveFormat.BytesPerSecond;
            int blockAlign = waveFormat.BlockAlign;

            double totalSeconds = (double)waveSource.Length / bytesPerSecond;
            double start = Math.Max(0, startSeconds.GetValueOrDefault(0));
            double end = Math.Min(endSeconds.GetValueOrDefault(totalSeconds), totalSeconds);
            double duration = Math.Max(0, end - start);

            long startBytes = (long)(start * bytesPerSecond);
            long bytesToWrite = (long)(duration * bytesPerSecond);

            // Align to block size
            startBytes -= startBytes % blockAlign;
            bytesToWrite -= bytesToWrite % blockAlign;

            // Skip to start
            waveSource.Position = startBytes;

            // Prepare MP3 writer
            using var mp3Stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            using var mp3Writer = new LameMP3FileWriter(
                mp3Stream,
                ConvertToNAudioFormat(waveFormat),
                128
            );

            var buffer = new byte[bytesPerSecond];
            while (bytesToWrite > 0)
            {
                int toRead = (int)Math.Min(buffer.Length, bytesToWrite);
                toRead -= toRead % blockAlign;
                if (toRead <= 0) break;

                int read = waveSource.Read(buffer, 0, toRead);
                if (read <= 0) break;

                mp3Writer.Write(buffer, 0, read);
                bytesToWrite -= read;
            }
        }

        private static NAudio.Wave.WaveFormat ConvertToNAudioFormat(CSCore.WaveFormat format)
        {
            return new NAudio.Wave.WaveFormat(format.SampleRate, format.BitsPerSample, format.Channels);
        }


        //public static void SaveAudioSegmentToFile(string outputPath, double? startSeconds = null, double? endSeconds = null, float gain = 1.0f)
        //{
        //    var sources = GetAudioSources();
        //    if (sources.Count == 0) return;

        //    var fullSource = new ChainedWaveSource(sources);
        //    fullSource.Position = 0;

        //    var waveFormat = fullSource.WaveFormat;
        //    long totalBytes = fullSource.Length;
        //    double totalSeconds = (double)totalBytes / waveFormat.BytesPerSecond;

        //    long startByte = 0;
        //    long endByte = totalBytes;

        //    if (startSeconds.HasValue && startSeconds.Value >= 0 && startSeconds.Value < totalSeconds)
        //        startByte = (long)(startSeconds.Value * waveFormat.BytesPerSecond);

        //    if (endSeconds.HasValue && endSeconds.Value > startSeconds.GetValueOrDefault() && endSeconds.Value <= totalSeconds)
        //        endByte = (long)(endSeconds.Value * waveFormat.BytesPerSecond);

        //    if (startByte >= totalBytes || endByte <= startByte)
        //    {
        //        startByte = 0;
        //        endByte = totalBytes;
        //    }

        //    fullSource.Position = startByte;
        //    long bytesToWrite = endByte - startByte;

        //    // Wrap in VolumeSource
        //    var sampleSource = fullSource.ToSampleSource();
        //    var volumeSource = new VolumeSource(sampleSource) { Volume = gain };
        //    var waveSource = volumeSource.ToWaveSource();

        //    using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        //    using var writer = new WaveWriter(fs, waveSource.WaveFormat);

        //    var buffer = new byte[waveSource.WaveFormat.BytesPerSecond];
        //    long actualBytesWritten = 0;

        //    while (bytesToWrite > 0)
        //    {
        //        int toRead = (int)Math.Min(buffer.Length, bytesToWrite);
        //        toRead -= toRead % waveSource.WaveFormat.BlockAlign;
        //        if (toRead <= 0) break;

        //        int read = waveSource.Read(buffer, 0, toRead);
        //        if (read <= 0) break;

        //        writer.Write(buffer, 0, read);
        //        actualBytesWritten += read;
        //        bytesToWrite -= read;
        //    }

        //    writer.Dispose();
        //}

        public static void StopAudio()
        {
            // Stop and dispose any previous playback
            try
            {
                _outputDevice?.Stop();
                _outputDevice?.Dispose();
            }
            catch { }

            _playbackSource?.Dispose();

            _outputDevice = null;
            _playbackSource = null;
        }

        public static bool IsPlaybackFinished =>
            _outputDevice == null || _outputDevice.PlaybackState == PlaybackState.Stopped;

        private static List<IWaveSource> GetAudioSources()
        {
            DeleteCorruptedFiles(); // first delete corrupteds before gathering audio
            return Directory.GetFiles(OutputDirectory, "audio_*.wav")
                            .OrderBy(f =>
                            {
                                var name = Path.GetFileNameWithoutExtension(f);
                                var digits = new string(name.Where(char.IsDigit).ToArray());
                                return int.TryParse(digits, out int num) ? num : 0;
                            })
                            .Select(file =>
                            {
                                var bytes = File.ReadAllBytes(file);
                                var stream = new MemoryStream(bytes);
                                return (IWaveSource)new WaveFileReader(stream);
                            })
                            .ToList();
        }
    }
}
