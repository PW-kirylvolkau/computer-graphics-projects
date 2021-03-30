using System;
using System.Drawing;
using System.Net.WebSockets;

namespace ComputerGraphics.Filters.Other
{
    public static class RandomDithering
    {
        private static readonly Random rand = new Random();
        public static Bitmap DitherRandomly(this Bitmap bmp, bool isGrayScale)
        {
            var tmp = new Bitmap(bmp);
            
            for (var i = 0; i < tmp.Width; i++)
            {
                for (var j = 0; j < tmp.Height; j++)
                {
                    var color = tmp.GetPixel(i, j);
                    var newColor = new Color();
                    if (isGrayScale)
                    {
                        var channel = color.R; 
                        var ditheredChannel = DitherChannel(channel);
                        newColor = Color.FromArgb(ditheredChannel, ditheredChannel, ditheredChannel);
                    }
                    else
                    {
                        var ditheredRed = DitherChannel(color.R);
                        var ditheredGreen = DitherChannel(color.G);
                        var ditheredBlue = DitherChannel(color.B);
                        newColor = Color.FromArgb(ditheredRed, ditheredGreen, ditheredBlue);
                    }
                    
                    tmp.SetPixel(i,j, newColor);
                }
            }

            return tmp;
        }

        public static int DitherChannel(int channel)
        {
            var number = rand.Next(1,256);
            var dithered = number > channel ? 0 : 255;

            return dithered;
        }
    }
}