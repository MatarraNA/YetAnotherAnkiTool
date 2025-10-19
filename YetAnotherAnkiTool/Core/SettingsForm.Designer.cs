namespace YetAnotherAnkiTool.Core
{
    partial class SettingsForm
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
            dreamForm1 = new ReaLTaiizor.Forms.DreamForm();
            cancelBtn = new ReaLTaiizor.Controls.DreamButton();
            saveBtn = new ReaLTaiizor.Controls.DreamButton();
            configPanel = new Panel();
            dreamForm1.SuspendLayout();
            SuspendLayout();
            // 
            // dreamForm1
            // 
            dreamForm1.ColorA = Color.FromArgb(40, 218, 255);
            dreamForm1.ColorB = Color.FromArgb(63, 63, 63);
            dreamForm1.ColorC = Color.FromArgb(41, 41, 41);
            dreamForm1.ColorD = Color.FromArgb(27, 27, 27);
            dreamForm1.ColorE = Color.FromArgb(0, 0, 0, 0);
            dreamForm1.ColorF = Color.FromArgb(25, 255, 255, 255);
            dreamForm1.Controls.Add(cancelBtn);
            dreamForm1.Controls.Add(saveBtn);
            dreamForm1.Controls.Add(configPanel);
            dreamForm1.Dock = DockStyle.Fill;
            dreamForm1.Location = new Point(0, 0);
            dreamForm1.Name = "dreamForm1";
            dreamForm1.Size = new Size(480, 360);
            dreamForm1.TabIndex = 0;
            dreamForm1.TabStop = false;
            dreamForm1.Text = "Settings";
            dreamForm1.TitleAlign = HorizontalAlignment.Center;
            dreamForm1.TitleHeight = 25;
            // 
            // cancelBtn
            // 
            cancelBtn.ColorA = Color.FromArgb(31, 31, 31);
            cancelBtn.ColorB = Color.FromArgb(41, 41, 41);
            cancelBtn.ColorC = Color.FromArgb(51, 51, 51);
            cancelBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            cancelBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            cancelBtn.ForeColor = Color.FromArgb(40, 218, 255);
            cancelBtn.Location = new Point(244, 314);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(224, 40);
            cancelBtn.TabIndex = 3;
            cancelBtn.Text = "Cancel";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // saveBtn
            // 
            saveBtn.ColorA = Color.FromArgb(31, 31, 31);
            saveBtn.ColorB = Color.FromArgb(41, 41, 41);
            saveBtn.ColorC = Color.FromArgb(51, 51, 51);
            saveBtn.ColorD = Color.FromArgb(0, 0, 0, 0);
            saveBtn.ColorE = Color.FromArgb(25, 255, 255, 255);
            saveBtn.ForeColor = Color.FromArgb(40, 218, 255);
            saveBtn.Location = new Point(12, 314);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(224, 40);
            saveBtn.TabIndex = 2;
            saveBtn.Text = "Save Config";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // configPanel
            // 
            configPanel.BackColor = Color.FromArgb(64, 64, 64);
            configPanel.Location = new Point(12, 40);
            configPanel.Name = "configPanel";
            configPanel.Size = new Size(456, 268);
            configPanel.TabIndex = 1;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 360);
            Controls.Add(dreamForm1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SettingsForm";
            Text = "SettingsForm";
            dreamForm1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Forms.DreamForm dreamForm1;
        private Panel configPanel;
        private ReaLTaiizor.Controls.DreamButton cancelBtn;
        private ReaLTaiizor.Controls.DreamButton saveBtn;
    }
}