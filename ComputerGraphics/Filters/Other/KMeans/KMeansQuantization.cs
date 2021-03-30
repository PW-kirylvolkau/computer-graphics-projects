using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ComputerGraphics.Filters.Other.KMeans
{
    public static class KMeansQuantization
    {
        private static readonly Random randomizer = new Random();
        
        // inspiration of the work with vectors was taken from here:
        // https://coderoad.ru/50665215/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC-K-%D1%81%D1%80%D0%B5%D0%B4%D0%BD%D0%B8%D1%85-C
        // (the website is in Russian)
        public static Bitmap ApplyKMeansQuantization(this Bitmap bmp, int k, int iterations)
        {
            var image = new Bitmap(bmp);
            var width = image.Width;
            var height = image.Height;

            // initialize centroids and corresponding clusters
            var centroids = new Vector[k];
            var clusters = new List<Vector>[k]; 

            for(var i = 0; i < k; i++)
            {
                var x = randomizer.Next(0, width);
                var y = randomizer.Next(0, height);
                var pixel = image.GetPixel(x, y);
                var centroid = Vector.FromColorAndCoords(pixel, x, y);

                centroids[i] = centroid; 
                
                var cluster = new List<Vector> { centroid };
                cluster.Add(centroid); 
                clusters[i] = cluster;
            }

            int count = 0;

            while (count < iterations) {
                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var pixel = image.GetPixel(x, y);
                        var currentVector = Vector.FromColorAndCoords(pixel, x, y);
                        var minDistance = double.PositiveInfinity;
                        var cluster = clusters[0];

                        //Finds cluster (via closes centroid)
                        for (var i = 0; i < centroids.Length; i++)
                        {
                            var centroid = centroids[i];
                            var distance = currentVector.Distance(centroid);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                cluster = clusters[i];
                            }
                        }

                        cluster.Add(currentVector);
                        var currentLength = cluster.Count;
                        var currentCentroid = cluster[0];

                        var newCentroid = new Vector(0, 0, 0);
                        
                        // update centroid
                        for (var i = 0; i < cluster.Count; i++)
                        {
                            var vector = cluster[i];
                            newCentroid.Sum(vector);
                        }
                        
                        var dr = newCentroid.R / currentLength;
                        var dg = newCentroid.G / currentLength;
                        var db = newCentroid.B / currentLength;
                        newCentroid = new Vector(dr, dg, db);
                        cluster[0] = newCentroid;
                        
                        for (var i = 0; i < centroids.Length; i++)
                        {
                            if (
                                centroids[i].R == currentCentroid.R 
                                && centroids[i].G == currentCentroid.G 
                                && currentCentroid.B == centroids[i].B)
                            {
                                centroids[i] = newCentroid;
                                break;
                            }
                        }
                    }
                }
                count++; 
            }
            
            // setting all values in cluster to the centroid 
            for(var i = 0; i < clusters.Length; i++ )
            {
                var cluster = clusters[i];
                var centoid = cluster[0];
                for(var j = 1; j < cluster.Count; j++)
                {
                    cluster[j].R = centoid.R;
                    cluster[j].G = centoid.G;
                    cluster[j].B = centoid.B;
                }
            }
            
            // building result image
            for (var i = 0; i < clusters.Length; i++)
            {
                var cluster = clusters[i];
                for (var j = 0; j < cluster.Count; j++)
                {
                    var vector = cluster[j];
                    var r = (int) vector.R;
                    var g = (int) vector.G;
                    var b = (int) vector.B;
                    var color = Color.FromArgb(r, g, b);
                    
                    image.SetPixel(vector.X, vector.Y, color);
                }
            }

            return image; 
        }
    }
}