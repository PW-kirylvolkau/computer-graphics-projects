using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Input.GestureRecognizers;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Microsoft.VisualBasic;

namespace ComputerGraphics.Views
{
    public class MainWindow : Window
    {
        private const int BRIGHTNESS_COEFF = 50;
        private const double GAMMA_COEFF = 0.5;
        
        private delegate int FunctionalFilter(int channel);
        private readonly FunctionalFilter _invertFilter = (channel) => 255 - channel;
        private readonly FunctionalFilter _brightenFilter = (channel) => 
        {
            var result = channel + BRIGHTNESS_COEFF;
            result = result > 255 ? 255 : result;
            result = result < 0 ? 0 : result;
            return result;
        };
        private readonly FunctionalFilter _gammaFilter = (channel) =>
        {
            var gammaCorrection = 1 / GAMMA_COEFF;
            var tmp = (double) channel / 255;
            var result = 255 * Math.Pow(tmp, gammaCorrection);
            result = result > 255 ? 255 : result;
            return (int) result;
        };
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        

        private void Experimanetalcanvas_OnTapped(object? sender, RoutedEventArgs e)
        {
            RenderCanvas();
        }

        public void RenderCanvas()
        {
            var canvas = this.FindControl<Canvas>("experimanetalcanvas");

            var line = new Polyline();
            var points = new List<Point>();
            for (var i = 0; i < 256; i++)
            {
                var point = new Point(i, 255 - _brightenFilter(i));
                points.Add(point);
            }

            line.Points = points;
            line.Stroke = SolidColorBrush.Parse("white");
            line.StrokeThickness = 5.0;
            line.StrokeJoin = PenLineJoin.Round;
            line.StrokeLineCap = PenLineCap.Round;

            var line2 = new Polyline();
            var points2 = new List<Point>();
            for (var i = 0; i < 256; i++)
            {
                var point = new Point(i, 255 - _gammaFilter(i));
                points2.Add(point);
            }

            line2.Points = points2;
            line2.Stroke = SolidColorBrush.Parse("black");
            canvas.Children.Add(line);
            canvas.Children.Add(line2);
            
            var circle = new Ellipse();
            DragDrop.SetAllowDrop(circle, true);
            circle.Fill = SolidColorBrush.Parse("green");
            circle.Height = 5;
            circle.Width = 5;
            canvas.Children.Add(circle);
            Canvas.SetTop(circle, 20);
            Canvas.SetLeft(circle, 20);
        }
    }
}