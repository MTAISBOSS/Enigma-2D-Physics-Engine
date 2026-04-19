using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Enigma_Framework.Utilities;

public static class ImageLoader
{
    public static void LoadImage(string path, out BitmapData data, out byte[] bytes)
    {
        using (var bmp = new Bitmap(path))
        {
            data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
            );

            int byteCount = System.Math.Abs(data.Stride) * data.Height;
            
            bytes = new byte[byteCount];
            Marshal.Copy(data.Scan0, bytes, 0, byteCount);
            bmp.UnlockBits(data);
        }
    }
}