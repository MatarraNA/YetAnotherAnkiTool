using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YetAnotherAnkiTool.Core.API
{
    public class ScreenshotAPI
    {
        public static string OUTPUT_PATH => Path.Combine(Environment.CurrentDirectory, "ScreenshotOut");
        private static int NUM_SS_TO_KEEP = 30;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static bool CaptureActiveWindow()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return false;

            if (!GetWindowRect(hWnd, out RECT rect)) return false;

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (width <= 0 || height <= 0) return false;

            // Ensure output folder exists
            Directory.CreateDirectory(OUTPUT_PATH);

            // Capture screenshot
            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using var gfx = Graphics.FromImage(bmp);
            gfx.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            // Apply size override if configured
            int overrideWidth = Config.Config.Configuration.AnkiImgWidthOverride;
            int overrideHeight = Config.Config.Configuration.AnkiImgHeightOverride;

            Bitmap finalImage = bmp;
            if (overrideWidth > 0 && overrideHeight > 0)
            {
                finalImage = new Bitmap(overrideWidth, overrideHeight);
                using var resizeGfx = Graphics.FromImage(finalImage);
                resizeGfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                resizeGfx.DrawImage(bmp, 0, 0, overrideWidth, overrideHeight);
            }

            string filePath = Path.Combine(OUTPUT_PATH, $"screenshot_{DateTime.Now.Ticks}.jpg");
            finalImage.Save(filePath, ImageFormat.Jpeg);

            if (finalImage != bmp)
                finalImage.Dispose(); // cleanup resized image

            // Cleanup: keep only the newest NUM_SS_TO_KEEP
            var files = Directory.GetFiles(OUTPUT_PATH, "screenshot_*.jpg")
                                 .OrderByDescending(File.GetLastWriteTime)
                                 .ToList();

            if (files.Count > NUM_SS_TO_KEEP)
            {
                foreach (var oldFile in files.Skip(NUM_SS_TO_KEEP))
                {
                    try { File.Delete(oldFile); } catch { /* ignore failures */ }
                }
            }

            return true;
        }
    }
}
