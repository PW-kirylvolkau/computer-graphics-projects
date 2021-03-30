using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ComputerGraphics.Filters.Other
{
    public static class GrayScale
    {
        public static Bitmap ToGrayScale(this Bitmap bmp)
        {
            var width = bmp.Width;
            var height = bmp.Height;

            var result = new Bitmap(bmp);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var color = result.GetPixel(i, j);
                    var gray = (int)(0.3 * color.R + 0.59 * color.G + 0.11 * color.B);
                    var grayColor = Color.FromArgb(gray, gray, gray);
                    result.SetPixel(i, j, grayColor);
                }
            }
            
            return result;
        }
    }
}