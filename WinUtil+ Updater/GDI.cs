using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WinUtilPlus
{
    public class GDI
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("kernel32.dll")]
        private static extern void Sleep(int dwMilliseconds);

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, uint rop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int color);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool Pie(IntPtr hdc, int left, int top, int right, int bottom, int xr1, int yr1, int xr2, int yr2);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private const int NOTSRCERASE = 0x001100A6;
        private static bool InvertTweenLeftTopEnabled = false;

        public static void InvertTweenLeftTop(bool Enabled) {
            if (Enabled == false)
            {
                InvertTweenLeftTopEnabled = false;
            }
            else {
                InvertTweenLeftTopEnabled = true;
            };
            while(InvertTweenLeftTopEnabled) {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int w = GetSystemMetrics(0);
                int h = GetSystemMetrics(1);
                BitBlt(hdc, new Random().Next(222), new Random().Next(222), w, h, hdc, new Random().Next(222), new Random().Next(222), NOTSRCERASE);
                Sleep(10);
                ReleaseDC(IntPtr.Zero, hdc);
            }
        }

        private static bool CubesEnabled = false;
        public static void Cubes(bool Enabled) {
            if(Enabled == false)
            {
                CubesEnabled = false;
            } else { CubesEnabled = true; }
            while(CubesEnabled)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int x = GetSystemMetrics(0);
                int y = GetSystemMetrics(1);
                StretchBlt(hdc, -10, -10, x + 20, y + 20, hdc, 0, 0, x, y, 0x00CC0020);
                StretchBlt(hdc, 10, 10, x - 20, y - 20, hdc, 0, 0, x, y, 0x00CC0020);
                ReleaseDC(IntPtr.Zero, hdc);
            }
        }

        public const int SRCCOPY = 0x00CC0020;
        private static bool TrainsEnabled = false;
        public static void Trains(bool Enabled)
        {
            TrainsEnabled = Enabled;
            int w = GetSystemMetrics(0);
            int h = GetSystemMetrics(1);
            while (TrainsEnabled)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                BitBlt(hdc, -30, 0, w, h, hdc, 0, 0, SRCCOPY);
                BitBlt(hdc, w - 30, 0, w, h, hdc, 0, 0, SRCCOPY);
                ReleaseDC(IntPtr.Zero, hdc);
                Thread.Sleep(10);
            }
        }

        private static bool darkrE = false;
        private static Random r1;
        public static void Darkr(bool Enabled)
        {
            darkrE = Enabled; 
            IntPtr hdc;
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1), x;
            while (darkrE)
            {
                r1 = new Random();
                int t = r1.Next(-20, 20);
                hdc = GetDC(IntPtr.Zero);
                x = new Random().Next(w);
                BitBlt(hdc, x, 1, 50, h * t, hdc, x, 0, 0x00CC0020);
                Thread.Sleep(2);
                ReleaseDC(IntPtr.Zero, hdc);
            }
        }

        private static bool rgbtrain = false;
        public static void RgbTrain(bool Enabled)
        {
            rgbtrain = Enabled;
            int w = GetSystemMetrics(0),
                h = GetSystemMetrics(1);
            while (rgbtrain)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                IntPtr brush = CreateSolidBrush((new Random()).Next(0, 256) | ((new Random()).Next(0, 256) << 8) | ((new Random()).Next(0, 256) << 16));
                SelectObject(hdc, brush);
                BitBlt(hdc, 0, 0, w, h, hdc, -30, 0, NOTSRCERASE);
                BitBlt(hdc, 0, 0, w, h, hdc, w - 30, 0, NOTSRCERASE);
                DeleteObject(brush);
                ReleaseDC(IntPtr.Zero, hdc);
            }
        }
    }
}
