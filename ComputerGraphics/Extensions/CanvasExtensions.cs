using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using ComputerGraphics.Filters.Functional;

namespace ComputerGraphics.Extensions
{
    public static class CanvasExtensions
    {
        public static Canvas DrawPolyline(this Canvas canvas, List<FunctionalFilter.FunctionalFilterDelegate> functions)
        {
            var points = new List<Point>();
            for (var i = 0; i < canvas.Height; i++)
            {
                var value= functions.Aggregate(i, (current, function) => function.Invoke(current));
                var point = new Point(i, 255 - value);
                points.Add(point);
            }

            var polyline = new Polyline()
            {
                Points = points,
                Stroke = Brushes.Red,
                StrokeThickness = 3,
                StrokeJoin = PenLineJoin.Round
            };
            
            canvas.Children.Add(polyline);

            return canvas;
        }
    }
}