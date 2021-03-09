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
using ComputerGraphics.ViewModels;
using Microsoft.VisualBasic;

namespace ComputerGraphics.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            MinWidth = 256 + 256 + 200 + 30;
            MinHeight = 256 + 256 + 200;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}