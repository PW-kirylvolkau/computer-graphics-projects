using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ComputerGraphics.Filters.Extensions;
using ComputerGraphics.Models;
using SystemBitmap = System.Drawing.Bitmap;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;

namespace ComputerGraphics.Extensions
{
    public static class BitmapExtensions
    {
        public static AvaloniaBitmap GetAvaloniaBitmap(this SystemBitmap bitmap)
        {
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;
            var bmp = new AvaloniaBitmap(stream);
            return bmp;
        }

        public static SystemBitmap GetSystemBitmap(this AvaloniaBitmap bitmap)
        {
            using var stream = new MemoryStream();
            bitmap.Save(stream);
            stream.Position = 0;
            var bmp = new SystemBitmap(stream);
            return bmp;
        }
    }
}