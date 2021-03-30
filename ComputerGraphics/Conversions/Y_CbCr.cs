using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ComputerGraphics.Conversions
{
    public class YCbCrColor
    {
        public byte Y { set; get; }
        public byte Cb { set; get; }
        public byte Cr { set; get; }
 
        public YCbCrColor(byte y, byte cb, byte cr) {
            Y = y;
            Cb = cb;
            Cr = cr;
        }
 
        public Color ToRgbColor()
        {
            var r = (int)((298.082 * Y + 408.583 * Cr) / 256.0 - 222.921);
            var g = (int)((298.082 * Y - 100.291 * Cb - 208.120 * Cr) / 256.0 + 135.576);
            var b = (int)((298.082 * Y + 516.412 * Cb) / 256.0 - 276.836);

            ClipColor(ref r);
            ClipColor(ref g);
            ClipColor(ref b);
 
            return Color.FromArgb(r, g, b);
        }
 
        public static YCbCrColor FromRgbColor(Color color) {
            var y =(byte)(16 + 1 / 256.0 * (65.738 * color.R + 129.057 * color.G + 25.064 * color.B));
            var cb = (byte) (128 + 1 / 256.0 * (112.439 * color.B - 74.494 * color.G - 37.945 * color.R));
            var cr = (byte) (128 + 1/256.0 * (112.439 * color.R - 94.154 * color.G -  18.285 * color.B));
            return new YCbCrColor(y, cb, cr);
        }
        
        public static void ClipColor(ref int color)
        {
            if (color > 255)
            {
                color = 255;
            }

            if (color < 0)
            {
                color = 0;
            }
        }
    }
    
    public static class Y_CbCr
    {
        private static readonly Random rand = new Random();

        public static Bitmap Step1(this Bitmap bmp)
        {
            Bitmap result = new(bmp);
            
            for (var i = 0; i < result.Width; i++)
            {
                for (var j = 0; j < result.Height; j++)
                {
                    var color = result.GetPixel(i, j);
                    var conv = YCbCrColor.FromRgbColor(color);

                    var dr = (int)conv.Y;
                    var dg = (int)conv.Cb;
                    var db = (int)conv.Cr;
                    
                    ClipColor(ref dr);
                    ClipColor(ref dg);
                    ClipColor(ref db);

                    var ycbcr = Color.FromArgb(dr, dg, db);
                    result.SetPixel(i, j, ycbcr);
                }
            }

            return result;
        }
        //
        // public static Bitmap Step2(this Bitmap bmp)
        // {
        //     Bitmap result = new(bmp);
        //     
        //     for (var i = 0; i < result.Width; i++)
        //     {
        //         for (var j = 0; j < result.Height; j++)
        //         {
        //             var color = result.GetPixel(i, j);
        //             var y = color.R;
        //             var g = color.
        //             var conv = new YCbCrColor();
        //
        //             var dr = (int)conv.Y;
        //             var dg = (int)conv.Cb;
        //             var db = (int)conv.Cr;
        //             
        //             ClipColor(ref dr);
        //             ClipColor(ref dg);
        //             ClipColor(ref db);
        //
        //             var ycbcr = Color.FromArgb(dr, dg, db);
        //             result.SetPixel(i, j, ycbcr);
        //         }
        //     }
        //
        //     return result;
        // }
        public static Bitmap ConvertToY_CbCr(this Bitmap original)
        {
            Bitmap result = new(original);
            
            for (var i = 0; i < result.Width; i++)
            {
                for (var j = 0; j < result.Height; j++)
                {
                    var color = result.GetPixel(i, j);
                    var conv = YCbCrColor.FromRgbColor(color);
                    var rgbColor = conv.ToRgbColor();
                    var r = rgbColor.R;
                    var b = rgbColor.B;
                    var g = rgbColor.G;
                    var dr = DitherChannel(r);
                    var dg = DitherChannel(g);
                    var db = DitherChannel(b);
                    
                    ClipColor(ref dr);
                    ClipColor(ref dg);
                    ClipColor(ref db);

                    var ditheredColor = Color.FromArgb(dr, dg, db);
                    result.SetPixel(i, j, ditheredColor);
                }
            }

            return result;
        }
        
        public static int DitherChannel(int channel)
        {
            var number = rand.Next(1,256);
            var dithered = number > channel ? 0 : 255;

            return dithered;
        }

        public static void ClipColor(ref int color)
        {
            if (color > 255)
            {
                color = 255;
            }

            if (color < 0)
            {
                color = 0;
            }
        }
    }
}