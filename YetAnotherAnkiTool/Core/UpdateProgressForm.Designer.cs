namespace YetAnotherAnkiTool.Core
{
    partial class UpdateProgressForm
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
            statusLabel = new ReaLTaiizor.Controls.FoxLabel();
            progressBar = new ProgressBar();
            SuspendLayout();
            // 
            // statusLabel
            // 
            statusLabel.BackColor = Color.Transparent;
            statusLabel.Dock = DockStyle.Bottom;
            statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            statusLabel.ForeColor = Color.FromArgb(192, 204, 216);
            statusLabel.Location = new Point(0, 62);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(384, 19);
            statusLabel.TabIndex = 10;
            statusLabel.Text = "0.00";
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Top;
            progressBar.Location = new Point(0, 0);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(384, 30);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 11;
            // 
            // UpdateProgressForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(41, 41, 41);
            ClientSize = new Size(384, 81);
            Controls.Add(progressBar);
            Controls.Add(statusLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UpdateProgressForm";
            Text = "Updater";
            Load += UpdateProgressForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private ReaLTaiizor.Controls.FoxLabel statusLabel;
        private ProgressBar progressBar;
    }
}