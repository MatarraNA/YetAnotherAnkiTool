using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updatum;
using YetAnotherAnkiTool.Core.API;

namespace YetAnotherAnkiTool.Core
{
    public partial class UpdateProgressForm : Form
    {
        public UpdateProgressForm()
        {
            InitializeComponent();
        }

        private void UpdateProgressForm_Load(object sender, EventArgs e)
        {
            GithubAPI.AppUpdater.PropertyChanged += OnUpdaterPropertyChanged;
        }

        private void OnUpdaterPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UpdatumManager.DownloadedPercentage))
            {
                Invoke(() =>
                {
                    progressBar.Value = (int)GithubAPI.AppUpdater.DownloadedPercentage;
                    statusLabel.Text = $"Downloaded {GithubAPI.AppUpdater.DownloadedMegabytes:F1} MB / {GithubAPI.AppUpdater.DownloadSizeMegabytes:F1} MB";
                    progressBar.Invalidate();
                    statusLabel.Invalidate();
                });
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Unsubscribe to avoid memory leaks
            GithubAPI.AppUpdater.PropertyChanged -= OnUpdaterPropertyChanged;
            base.OnFormClosed(e);
        }
    }
}
