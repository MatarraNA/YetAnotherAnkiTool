using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherAnkiTool.Core.Control
{
    public class TrackBarOverlay : UserControl
    {
        public Func<int>? GetStartValue;
        public Func<int>? GetEndValue;
        public int Minimum => BoundTrackBar != null ? BoundTrackBar.Minimum : 0;
        public int Maximum => BoundTrackBar != null ? BoundTrackBar.Maximum : 0;
        public ReaLTaiizor.Controls.TrackBar? BoundTrackBar { get; set; }

        public TrackBarOverlay()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Paint the underlying TrackBar
            if (BoundTrackBar != null)
            {
                using (var bmp = new Bitmap(BoundTrackBar.Width, BoundTrackBar.Height))
                {
                    BoundTrackBar.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    g.DrawImageUnscaled(bmp, 0, 0);
                }
            }

            int trackWidth = Width;

            int startValue = GetStartValue?.Invoke() ?? Minimum;
            int endValue = GetEndValue?.Invoke() ?? Maximum;

            startValue = Math.Max(Minimum, Math.Min(Maximum, startValue));
            endValue = Math.Max(Minimum, Math.Min(Maximum, endValue));

            float startX = (float)(trackWidth * (startValue - Minimum) / (Maximum - Minimum));
            float endX = (float)(trackWidth * (endValue - Minimum) / (Maximum - Minimum));

            // Outline pens (draw first)
            var outlinePen = new Pen(Color.Black, 5); // thicker, darker base

            g.DrawLine(outlinePen, endX, 0, endX, Height);
            g.DrawLine(outlinePen, startX, 0, startX, Height);

            // Foreground pens (draw second)
            var penEnd = new Pen(Color.OrangeRed, 3);
            var penStart = new Pen(Color.LimeGreen, 3);

            g.DrawLine(penEnd, endX, 0, endX, Height);
            g.DrawLine(penStart, startX, 0, startX, Height);
        }

    }
}
