using System;
using System.Drawing;

namespace ComputerGraphics.Filters.Other.KMeans
{
    public class Vector
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public void SetCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double Distance(Vector v2)
        {
            var r_power = Math.Pow(R - v2.R, 2);
            var g_power = Math.Pow(G - v2.G, 2);
            var b_power = Math.Pow(B - v2.B, 2);
            
            var distance = Math.Sqrt(r_power + g_power + b_power);

            return distance; 
        }

        public double Length()
        {
            var length = Math.Sqrt(Math.Pow(R, 2) + Math.Pow(G, 2) + Math.Pow(B, 2));

            return length;
        }

        public void Sum(Vector v2)
        {
            R += v2.R; 
            G += v2.G;
            B += v2.B;
        }

        public static Vector FromColorAndCoords(Color color, int x, int y)
        {
            var vector = new Vector(color.R, color.G, color.B);
            vector.SetCoordinates(x,y);

            return vector;
        }
    }
}