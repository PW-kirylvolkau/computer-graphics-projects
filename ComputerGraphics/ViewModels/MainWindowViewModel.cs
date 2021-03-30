using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ComputerGraphics.Conversions;
using ComputerGraphics.Extensions;
using ComputerGraphics.Filters.Extensions;
using ComputerGraphics.Filters.Convolutional;
using ComputerGraphics.Filters.Functional;
using ComputerGraphics.Filters.Other;
using ComputerGraphics.Filters.Other.KMeans;
using ComputerGraphics.Models;
using ReactiveUI;
using SystemBitmap = System.Drawing.Bitmap;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;
using SystemImage = System.Drawing.Image;

namespace ComputerGraphics.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly string _canvasName = "FunctionCanvas";

        private delegate void FilterApplied(FunctionalFilter.FunctionalFilterDelegate filterMethod);

        private event FilterApplied OnFunctionalFilterApplied;

        private string? _path;
        private int _k = 4;
        private int _iterations = 5;
        private string _customFilterName = "[No name filter]";
        private bool _isGrayScale = false;
        private bool _applyingKMeans = false;
        private SystemBitmap? _originalImage;
        private SystemBitmap? _activeImage;
        private CustomFilter? _customFilter;
        private readonly List<CustomFilter> _customFilters;

        public MainWindowViewModel()
        {
            _customFilters = new List<CustomFilter>();
            OnFunctionalFilterApplied = new FilterApplied(FunctionalFilterApplied);
        }

        public string? Path
        {
            get => _path;
            private set => this.RaiseAndSetIfChanged(ref _path, value);
        }

        public AvaloniaBitmap? OriginalImage
        {
            get => _originalImage?.GetAvaloniaBitmap();
            private set => this.RaiseAndSetIfChanged(ref _originalImage, value?.GetSystemBitmap());
        }

        public AvaloniaBitmap? ActiveImage
        {
            get => _activeImage?.GetAvaloniaBitmap();
            set => this.RaiseAndSetIfChanged(ref _activeImage, value?.GetSystemBitmap());
        }

        public CustomFilter? CustomFilter
        {
            get => _customFilter;
            set => this.RaiseAndSetIfChanged(ref _customFilter, value);
        }

        public string CustomFilterName
        {
            get => _customFilterName;
            set => this.RaiseAndSetIfChanged(ref _customFilterName, value);
        }

        public int K
        {
            get => _k; 
            set => this.RaiseAndSetIfChanged(ref _k, value);
        }

        public int Iterations
        {
            get => _iterations;
            set => this.RaiseAndSetIfChanged(ref _iterations, value);
        }

        public bool ApplyingKMeans
        {
            get => _applyingKMeans;
            set => this.RaiseAndSetIfChanged(ref _applyingKMeans, value);
        }

        #region home_task project 2

        public async Task GrayScale()
        {
            var pic = await Task.Run(() => _activeImage?.ToGrayScale());
            _activeImage = pic;
            _isGrayScale = true;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async Task DitherRandomly()
        {
            var pic = await Task.Run(() => _activeImage?.DitherRandomly(_isGrayScale));
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async Task KMeans()
        {
            _applyingKMeans = true;
            this.RaisePropertyChanged(nameof(ApplyingKMeans));
            var pic = await Task.Run(() => _activeImage?.ApplyKMeansQuantization(_k, _iterations));
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
            _applyingKMeans = false;
            this.RaisePropertyChanged(nameof(ApplyingKMeans));
        }
        
        #endregion
        
        #region lab_task project 2

        public async Task ApplyLabPart()
        {
            var pic = await Task.Run(() => _activeImage!.ConvertToY_CbCr());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        #endregion

        #region lab_task project 1

        private double _a1;

        public double A1
        {
            get => _a1;
            set => _a1 = this.RaiseAndSetIfChanged(ref _a1, value);
        }

        private double _a2;

        public double A2
        {
            get => _a2;
            set => _a2 = this.RaiseAndSetIfChanged(ref _a2, value);
        }

        private double _a3;

        public double A3
        {
            get => _a3;
            set => _a3 = this.RaiseAndSetIfChanged(ref _a3, value);
        }

        private double _a4;

        public double A4
        {
            get => _a4;
            set => _a4 = this.RaiseAndSetIfChanged(ref _a4, value);
        }

        private double _a5;

        public double A5
        {
            get => _a5;
            set => _a5 = this.RaiseAndSetIfChanged(ref _a5, value);
        }

        private double _a6;

        public double A6
        {
            get => _a6;
            set => _a6 = this.RaiseAndSetIfChanged(ref _a6, value);
        }

        private double _a7;

        public double A7
        {
            get => _a7;
            set => _a7 = this.RaiseAndSetIfChanged(ref _a7, value);
        }

        private double _a8;

        public double A8
        {
            get => _a8;
            set => _a8 = this.RaiseAndSetIfChanged(ref _a8, value);
        }

        private double _a9;

        public double A9
        {
            get => _a9;
            set => _a9 = this.RaiseAndSetIfChanged(ref _a9, value);
        }

        public async void ApplyLabFilter()
        {
            var pic = new SystemBitmap(_activeImage);
            await Task.Run(() =>
            {
                for (var i = 0; i < pic.Width; i++)
                {
                    for (var j = 0; j < pic.Height; j++)
                    {
                        var px = pic.GetPixel(i, j);
                        var dr = A1 * px.R + A2 * px.G + A3 * px.B;
                        var dg = A4 * px.R + A5 * px.G + A6 * px.B;
                        var db = A7 * px.R + A8 * px.G + A9 * px.B;
                        ClipChannel(ref dr);
                        ClipChannel(ref dg);
                        ClipChannel(ref db);
                        var color = Color.FromArgb(255, (int) dr, (int) dg, (int) db);
                        pic.SetPixel(i, j, color);
                    }
                }
            });

            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));


            static void ClipChannel(ref double channel)
            {
                if (channel > 255)
                {
                    channel = 255;
                }

                if (channel < 0)
                {
                    channel = 0;
                }
            }
        }

        #endregion

        #region image file methods

        public async void SelectImage()
        {
            var filter = new FileDialogFilter() { Name = "Images", Extensions = {"svg", "jpg", "jpeg", "png"} };
            var dialog = new OpenFileDialog() { AllowMultiple = false, Filters = new List<FileDialogFilter>() {filter}};

            var window = GetMainWindow();

            var result = await dialog.ShowAsync(window);
            var path = result.FirstOrDefault();
            if (path is not null)
            {
                Path = path;
                OriginalImage = new AvaloniaBitmap(path);
                ActiveImage = new AvaloniaBitmap(path);
            }
            ResetCanvasAndActiveCustomFilter();
        }
        
        public void RestoreImage()
        {
            _activeImage = _originalImage;
            _isGrayScale = false;
            ResetCanvasAndActiveCustomFilter();
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public async void SaveImage()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new() {Name = "JPG", Extensions = {"jpeg"}});
            dialog.Filters.Add(new() {Name = "PNG", Extensions = {"png"}});
            dialog.Filters.Add(new() {Name = "SVG", Extensions = {"svg"}});

            var window = GetMainWindow();

            var result = await dialog.ShowAsync(window);

            if (result is not null)
            {
                _activeImage?.Save(result);
            }
        }

        #endregion

        #region functional filters

        private async Task ApplyFunctionalFilter<T>() 
            where T : FunctionalFilter, new()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyFunctionalFilter<T>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
            var filter = new T();
            OnFunctionalFilterApplied.Invoke(filter.Filter);
        }

        public async void InvertImage() => await ApplyFunctionalFilter<Inversion>();
        public async void BrightenImage() => await ApplyFunctionalFilter<Brightening>();
        public async void ContrastImage() => await ApplyFunctionalFilter<Contrast>();
        public async void GammaImage() => await ApplyFunctionalFilter<Gamma>();

        #endregion

        #region convolutional filters

        private async Task ApplyConvolutionalFilter<T>() 
            where T: ConvolutionalFilter, new()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<T>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async void BlurImage() => await ApplyConvolutionalFilter<Blur>();
        public async void IdentityImage () => await ApplyConvolutionalFilter<Identity>();
        public async void EdgeDetection() => await ApplyConvolutionalFilter<EdgeDetection>();
        public async void Emboss() => await ApplyConvolutionalFilter<Emboss>();
        public async void GaussianBlur() => await ApplyConvolutionalFilter<GaussianBlur>();
        public async void SharpenImage() => await ApplyConvolutionalFilter<Sharpen>();

        #endregion

        #region functional filters gui

        private Canvas GetFunctionCanvas()
        {
            var window = GetMainWindow();
            var canvas = window.FindControl<Canvas>(_canvasName);

            return canvas;
        }
        
        private void ResetCanvasAndActiveCustomFilter()
        {
            CustomFilter = null;
            GetFunctionCanvas().Children.Clear();
        }

        #endregion

        #region custom filter methods

        public void SaveFilter()
        {
            //TODO: input validation.
            CustomFilter!.Name = CustomFilterName;
            _customFilters.Add(CustomFilter);

            var stackPanel = GetMainWindow().FindControl<StackPanel>("CustomFunctionalFiltersPanel");

            var button = new Button();
            button.Content = CustomFilter.Name;
            button.CommandParameter = _customFilters.Count - 1;
            button.Command = ReactiveCommand.Create<int>((idx) =>
            {
                var buttonFilter = _customFilters[idx];
                if (CustomFilter is null)
                {
                    CustomFilter = buttonFilter;
                }
                else
                {
                    CustomFilter
                        .FunctionalFilters
                        .AddRange(buttonFilter.FunctionalFilters);
                }
                VisualizeFilter(CustomFilter);
                _activeImage = _activeImage!.ApplyCustomFunctionalFilter(CustomFilter);
                this.RaisePropertyChanged(nameof(ActiveImage));
            });

            stackPanel.Children.Add(button);
        }
        
        private void FunctionalFilterApplied(FunctionalFilter.FunctionalFilterDelegate filterDelegate)
        {
            CustomFilter ??= new CustomFilter();
            CustomFilter.FunctionalFilters.Add(filterDelegate);

            VisualizeFilter(CustomFilter);
        }
        
        private void VisualizeFilter(CustomFilter filter)
        {
            var canvas = GetFunctionCanvas();
            canvas.Children.Clear();
            canvas.DrawPolyline(filter.FunctionalFilters);
        }

        #endregion
        
        private Window GetMainWindow()
        {
            var desktop = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var window = desktop!.MainWindow;

            return window;
        }
    }
}