namespace YetAnotherAnkiTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            audioSeekTrackBar = new ReaLTaiizor.Controls.TrackBar();
            YetAnotherAnkiTool = new ReaLTaiizor.Forms.DreamForm();
            imageFlowPanel = new FlowLayoutPanel();
            menuMinimizeBtn = new ReaLTaiizor.Controls.DreamButton();
            menuCloseBtn = new ReaLTaiizor.Controls.DreamButton();
            sendToAnkiBtn = new ReaLTaiizor.Controls.DreamButton();
            settingsBtn = new ReaLTaiizor.Controls.DreamButton();
            groupBox1 = new GroupBox();
            ankiNoteIdLabel = new ReaLTaiizor.Controls.FoxLabel();
            ankiWordLabel = new ReaLTaiizor.Controls.FoxLabel();
            statusLabel = new ReaLTaiizor.Controls.FoxLabel();
            trackPosLabel = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel2 = new ReaLTaiizor.Controls.FoxLabel();
            foxLabel1 = new ReaLTaiizor.Controls.FoxLabel();
            endOffsetTxtBox = new ReaLTaiizor.Controls.DreamTextBox();
            startOffsetTxtBox = new ReaLTaiizor.Controls.DreamTextBox();
            playAudioBtn = new ReaLTaiizor.Controls.DreamButton();
            YetAnotherAnkiTool.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // audioSeekTrackBar
            // 
            audioSeekTrackBar.JumpToMouse = false;
            audioSeekTrackBar.Location = new Point(12, 407);
            audioSeekTrackBar.Maximum = 10;
            audioSeekTrackBar.Minimum = 0;
            audioSeekTrackBar.MinimumSize = new Size(47, 22);
            audioSeekTrackBar.Name = "audioSeekTrackBar";
            audioSeekTrackBar.Size = new Size(680, 22);
            audioSeekTrackBar.TabIndex = 0;
            audioSeekTrackBar.Text = "trackBar1";
            audioSeekTrackBar.Value = 0;
            audioSeekTrackBar.ValueDivison = ReaLTaiizor.Controls.TrackBar.ValueDivisor.By1;
            audioSeekTrackBar.ValueToSet = 0F;
            audioSeekTrackBar.ValueChanged += audioSeekTrackBar_ValueChanged;
            audioSeekTrackBar.MouseDown += audioSeekTrackBar_MouseDown;
            audioSeekTrackBar.MouseMove += audioSeekTrackBar_MouseMove;
            audioSeekTrackBar.MouseUp += audioSeekTrackBar_MouseUp;
            // 
            // YetAnotherAnkiTool
            // 
            YetAnotherAnkiTool.ColorA = Color.FromArgb(40, 218, 255);
            YetAnotherAnkiTool.ColorB = Color.FromArgb(63, 63, 63);
            YetAnotherAnkiTool.ColorC = Color.FromArgb(41, 41, 41);
            YetAnotherAnkiTool.ColorD = Color.FromArgb(27, 27, 27);
            YetAnotherAnkiTool.ColorE = Color.FromArgb(0, 0, 0, 0);
            YetAnotherAnkiTool.ColorF = Color.FromArgb(25, 255, 255, 255);
            YetAnotherAnkiTool.Controls.Add(imageFlowPanel);
            YetAnotherAnkiTool.Controls.Add(menuMinimizeBtn);
            YetAnotherAnkiTool.Controls.Add(menuCloseBtn);
            YetAnotherAnkiTool.Controls.Add(sendToAnkiBtn);
            YetAnotherAnkiTool.Controls.Add(settingsBtn);
            YetAnotherAnkiTool.Controls.Add(groupBox1);
            YetAnotherAnkiTool.Controls.Add(statusLabel);
            YetAnotherAnkiTool.Controls.Add(trackPosLabel);
            YetAnotherAnkiTool.Controls.Add(foxLabel2);
            YetAnotherAnkiTool.Controls.Add(foxLabel1);
            YetAnotherAnkiTool.Controls.Add(endOffsetTxtBox);
            YetAnotherAnkiTool.Controls.Add(startOffsetTxtBox);
            YetAnotherAnkiTool.Controls.Add(playAudioBtn);
            YetAnotherAnkiTool.Controls.Add(audioSeekTrackBar);
            YetAnotherAnkiTool.Dock = DockStyle.Fill;
            YetAnotherAnkiTool.Location = new Point(0, 0);
            YetAnotherAnkiTool.Name = "YetAnotherAnkiTool";
            YetAnotherAnkiTool.Size = new Size(704, 441);
            YetAnotherAnkiTool.TabIndex = 1;
            YetAnotherAnkiTool.TabStop = false;
            YetAnotherAnkiTool.Text = "Yet Another Anki Tool";
            YetAnotherAnkiTool.TitleAlign = HorizontalAlignment.Center;
            YetAnotherAnkiTool.TitleHeight = 25;
            YetAnotherAnkiTool.Enter += mainForm_Enter;
            // 
            // imageFlowPanel
            // 
            imageFlowPanel.AutoScroll = true;
            imageFlowPanel.BackColor = Color.FromArgb(41, 41, 41);
            imageFlowPanel.BorderStyle = BorderStyle.FixedSingle;
            imageFlowPanel.Location = new Point(12, 122);
            imageFlowPanel.Name = "imageFlowPanel";
            imageFlowPanel.Size = new Size(680, 200);
            imageFlowPanel.TabIndex = 19;
            imageFlowPanel.WrapContents = false;
            // 
            // menuMinimizeBtn
            // 
            menuMinimizeBtn.ColorA = Color.FromArgb(31, 31, 31);
            menuMinimizeBtn.ColorB = Color.FromArgb(41, 41, 41);
            menuMinimizeBtn.ColorC = Color.FromArgb(51, 51, 51);
            menuMinimizeBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            menuMinimizeBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            menuMinimizeBtn.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);
            menuMinimizeBtn.ForeColor = Color.FromArgb(60, 255, 140);
            menuMinimizeBtn.Location = new Point(655, 2);
            menuMinimizeBtn.Name = "menuMinimizeBtn";
            menuMinimizeBtn.Size = new Size(20, 20);
            menuMinimizeBtn.TabIndex = 18;
            menuMinimizeBtn.Text = "_";
            menuMinimizeBtn.UseVisualStyleBackColor = true;
            menuMinimizeBtn.Click += menuMinimizeBtn_Click;
            // 
            // menuCloseBtn
            // 
            menuCloseBtn.ColorA = Color.FromArgb(31, 31, 31);
            menuCloseBtn.ColorB = Color.FromArgb(41, 41, 41);
            menuCloseBtn.ColorC = Color.FromArgb(51, 51, 51);
            menuCloseBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            menuCloseBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            menuCloseBtn.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);
            menuCloseBtn.ForeColor = Color.FromArgb(255, 80, 100);
            menuCloseBtn.Location = new Point(681, 2);
            menuCloseBtn.Name = "menuCloseBtn";
            menuCloseBtn.Size = new Size(20, 20);
            menuCloseBtn.TabIndex = 17;
            menuCloseBtn.Text = "X";
            menuCloseBtn.UseVisualStyleBackColor = true;
            menuCloseBtn.Click += menuCloseBtn_Click;
            // 
            // sendToAnkiBtn
            // 
            sendToAnkiBtn.ColorA = Color.FromArgb(31, 31, 31);
            sendToAnkiBtn.ColorB = Color.FromArgb(41, 41, 41);
            sendToAnkiBtn.ColorC = Color.FromArgb(51, 51, 51);
            sendToAnkiBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            sendToAnkiBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            sendToAnkiBtn.Font = new Font("Segoe UI", 9F);
            sendToAnkiBtn.ForeColor = Color.FromArgb(40, 218, 255);
            sendToAnkiBtn.Location = new Point(461, 371);
            sendToAnkiBtn.Name = "sendToAnkiBtn";
            sendToAnkiBtn.Size = new Size(94, 30);
            sendToAnkiBtn.TabIndex = 16;
            sendToAnkiBtn.Text = "Send to Anki";
            sendToAnkiBtn.UseVisualStyleBackColor = true;
            sendToAnkiBtn.Click += sendToAnkiBtn_Click;
            // 
            // settingsBtn
            // 
            settingsBtn.ColorA = Color.FromArgb(31, 31, 31);
            settingsBtn.ColorB = Color.FromArgb(41, 41, 41);
            settingsBtn.ColorC = Color.FromArgb(51, 51, 51);
            settingsBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            settingsBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            settingsBtn.Font = new Font("Segoe UI", 12F);
            settingsBtn.ForeColor = Color.FromArgb(40, 218, 255);
            settingsBtn.Location = new Point(667, 40);
            settingsBtn.Name = "settingsBtn";
            settingsBtn.Size = new Size(25, 25);
            settingsBtn.TabIndex = 15;
            settingsBtn.Text = "⚙";
            settingsBtn.UseVisualStyleBackColor = true;
            settingsBtn.Click += settingsBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(41, 41, 41);
            groupBox1.Controls.Add(ankiNoteIdLabel);
            groupBox1.Controls.Add(ankiWordLabel);
            groupBox1.ForeColor = Color.FromArgb(192, 204, 216);
            groupBox1.Location = new Point(12, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(252, 76);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Text = "Latest Anki Card";
            // 
            // ankiNoteIdLabel
            // 
            ankiNoteIdLabel.BackColor = Color.Transparent;
            ankiNoteIdLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            ankiNoteIdLabel.ForeColor = Color.FromArgb(192, 204, 216);
            ankiNoteIdLabel.Location = new Point(6, 22);
            ankiNoteIdLabel.Name = "ankiNoteIdLabel";
            ankiNoteIdLabel.Size = new Size(240, 19);
            ankiNoteIdLabel.TabIndex = 12;
            ankiNoteIdLabel.Text = "Awaiting New Card";
            // 
            // ankiWordLabel
            // 
            ankiWordLabel.BackColor = Color.Transparent;
            ankiWordLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            ankiWordLabel.ForeColor = Color.FromArgb(192, 204, 216);
            ankiWordLabel.Location = new Point(6, 47);
            ankiWordLabel.Name = "ankiWordLabel";
            ankiWordLabel.Size = new Size(240, 19);
            ankiWordLabel.TabIndex = 13;
            ankiWordLabel.Text = "...";
            // 
            // statusLabel
            // 
            statusLabel.BackColor = Color.Transparent;
            statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            statusLabel.ForeColor = Color.FromArgb(224, 224, 224);
            statusLabel.Location = new Point(5, 3);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(253, 19);
            statusLabel.TabIndex = 11;
            statusLabel.Text = "Status: Capturing";
            // 
            // trackPosLabel
            // 
            trackPosLabel.BackColor = Color.Transparent;
            trackPosLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            trackPosLabel.ForeColor = Color.FromArgb(192, 204, 216);
            trackPosLabel.Location = new Point(632, 382);
            trackPosLabel.Name = "trackPosLabel";
            trackPosLabel.Size = new Size(60, 19);
            trackPosLabel.TabIndex = 9;
            trackPosLabel.Text = "0.00";
            // 
            // foxLabel2
            // 
            foxLabel2.BackColor = Color.Transparent;
            foxLabel2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            foxLabel2.ForeColor = Color.FromArgb(192, 204, 216);
            foxLabel2.Location = new Point(138, 353);
            foxLabel2.Name = "foxLabel2";
            foxLabel2.Size = new Size(120, 19);
            foxLabel2.TabIndex = 8;
            foxLabel2.Text = "End Offset:";
            // 
            // foxLabel1
            // 
            foxLabel1.BackColor = Color.Transparent;
            foxLabel1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            foxLabel1.ForeColor = Color.FromArgb(192, 204, 216);
            foxLabel1.Location = new Point(12, 353);
            foxLabel1.Name = "foxLabel1";
            foxLabel1.Size = new Size(120, 19);
            foxLabel1.TabIndex = 7;
            foxLabel1.Text = "Start Offset:";
            // 
            // endOffsetTxtBox
            // 
            endOffsetTxtBox.BackColor = Color.FromArgb(41, 41, 41);
            endOffsetTxtBox.BorderStyle = BorderStyle.FixedSingle;
            endOffsetTxtBox.ColorA = Color.FromArgb(31, 31, 31);
            endOffsetTxtBox.ColorB = Color.FromArgb(41, 41, 41);
            endOffsetTxtBox.ColorC = Color.FromArgb(51, 51, 51);
            endOffsetTxtBox.ColorD = Color.FromArgb(0, 0, 0, 0);
            endOffsetTxtBox.ColorE = Color.FromArgb(25, 255, 255, 255);
            endOffsetTxtBox.ColorF = Color.Black;
            endOffsetTxtBox.ForeColor = Color.FromArgb(40, 218, 255);
            endOffsetTxtBox.Location = new Point(138, 378);
            endOffsetTxtBox.Name = "endOffsetTxtBox";
            endOffsetTxtBox.ReadOnly = true;
            endOffsetTxtBox.Size = new Size(120, 23);
            endOffsetTxtBox.TabIndex = 6;
            endOffsetTxtBox.Text = "0.00";
            // 
            // startOffsetTxtBox
            // 
            startOffsetTxtBox.BackColor = Color.FromArgb(41, 41, 41);
            startOffsetTxtBox.BorderStyle = BorderStyle.FixedSingle;
            startOffsetTxtBox.ColorA = Color.FromArgb(31, 31, 31);
            startOffsetTxtBox.ColorB = Color.FromArgb(41, 41, 41);
            startOffsetTxtBox.ColorC = Color.FromArgb(51, 51, 51);
            startOffsetTxtBox.ColorD = Color.FromArgb(0, 0, 0, 0);
            startOffsetTxtBox.ColorE = Color.FromArgb(25, 255, 255, 255);
            startOffsetTxtBox.ColorF = Color.Black;
            startOffsetTxtBox.ForeColor = Color.FromArgb(40, 218, 255);
            startOffsetTxtBox.Location = new Point(12, 378);
            startOffsetTxtBox.Name = "startOffsetTxtBox";
            startOffsetTxtBox.ReadOnly = true;
            startOffsetTxtBox.Size = new Size(120, 23);
            startOffsetTxtBox.TabIndex = 5;
            startOffsetTxtBox.Text = "0.00";
            // 
            // playAudioBtn
            // 
            playAudioBtn.ColorA = Color.FromArgb(31, 31, 31);
            playAudioBtn.ColorB = Color.FromArgb(41, 41, 41);
            playAudioBtn.ColorC = Color.FromArgb(51, 51, 51);
            playAudioBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            playAudioBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            playAudioBtn.Font = new Font("Segoe UI", 16F);
            playAudioBtn.ForeColor = Color.FromArgb(40, 218, 255);
            playAudioBtn.Location = new Point(337, 371);
            playAudioBtn.Name = "playAudioBtn";
            playAudioBtn.Size = new Size(30, 30);
            playAudioBtn.TabIndex = 4;
            playAudioBtn.Text = "▶";
            playAudioBtn.UseVisualStyleBackColor = true;
            playAudioBtn.Click += playAudioBtn_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 441);
            Controls.Add(YetAnotherAnkiTool);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(2560, 1392);
            MinimumSize = new Size(261, 65);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YetAnotherAnkiTool";
            TransparencyKey = Color.Fuchsia;
            YetAnotherAnkiTool.ResumeLayout(false);
            YetAnotherAnkiTool.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.TrackBar audioSeekTrackBar;
        private ReaLTaiizor.Forms.DreamForm YetAnotherAnkiTool;
        private ReaLTaiizor.Controls.DreamButton playAudioBtn;
        private ReaLTaiizor.Controls.DreamTextBox startOffsetTxtBox;
        private ReaLTaiizor.Controls.FoxLabel foxLabel2;
        private ReaLTaiizor.Controls.FoxLabel foxLabel1;
        private ReaLTaiizor.Controls.DreamTextBox endOffsetTxtBox;
        private ReaLTaiizor.Controls.FoxLabel statusLabel;
        private ReaLTaiizor.Controls.FoxLabel trackPosLabel;
        private ReaLTaiizor.Controls.FoxLabel ankiNoteIdLabel;
        private ReaLTaiizor.Controls.FoxLabel ankiWordLabel;
        private GroupBox groupBox1;
        private ReaLTaiizor.Controls.DreamButton settingsBtn;
        private ReaLTaiizor.Controls.DreamButton sendToAnkiBtn;
        private ReaLTaiizor.Controls.DreamButton menuCloseBtn;
        private ReaLTaiizor.Controls.DreamButton menuMinimizeBtn;
        private FlowLayoutPanel imageFlowPanel;
    }
}