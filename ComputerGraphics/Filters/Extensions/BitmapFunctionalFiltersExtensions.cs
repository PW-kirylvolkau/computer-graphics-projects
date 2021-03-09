using System.Drawing;
using ComputerGraphics.Filters.Functional;
using ComputerGraphics.Models;

namespace ComputerGraphics.Filters.Extensions
{
    public static class BitmapFunctionalFiltersExtensions
    {
        public static Bitmap ApplyFunctionalFilter<T>(this Bitmap original) 
            where T : FunctionalFilter, new()
        {
            var filter = new T();
            
            var pic = new Bitmap(original);
            
            for (var y = 0; y <= pic.Height - 1; y++)
            {
                for (var x = 0; x <= pic.Width - 1; x++)
                {
                    var pixel = pic.GetPixel(x, y);
                    ApplyFunctionalFilterToPixel(ref pixel, filter.Filter);
                    pic.SetPixel(x, y, pixel);
                }
            }

            return pic;
        }

        public static Bitmap ApplyCustomFunctionalFilter(
            this Bitmap original,
            CustomFilter customFilter)
        {
            var pic = new Bitmap(original);
            
            for (var y = 0; y <= pic.Height - 1; y++)
            {
                for (var x = 0; x <= pic.Width - 1; x++)
                {
                    var pixel = pic.GetPixel(x, y);
                    foreach (var filter in customFilter.FunctionalFilters)
                    {
                        ApplyFunctionalFilterToPixel(ref pixel, filter);
                    }
                    pic.SetPixel(x, y, pixel);
                }
            }

            return pic;
        }
        
        private static void ApplyFunctionalFilterToPixel(ref Color color, FunctionalFilter.FunctionalFilterDelegate filter)
        {
            var red = filter(color.R);
            var green = filter(color.G);
            var blue = filter(color.B);
            color = Color.FromArgb(255, red, green, blue);
        }
    }
}