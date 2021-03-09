using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ComputerGraphics.Extensions;
using ComputerGraphics.Filters.Extensions;
using ComputerGraphics.Filters.Convolutional;
using ComputerGraphics.Filters.Functional;
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
        private string _customFilterName = "[No name filter]";
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