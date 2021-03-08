using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using ComputerGraphics.Extensions;
using ComputerGraphics.Filters;
using ComputerGraphics.Filters.Convolutional;
using ReactiveUI;
using SystemBitmap = System.Drawing.Bitmap;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;
using SystemImage = System.Drawing.Image;

namespace ComputerGraphics.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const int BRIGHTNESS_COEFF = 10;
        private const int CONTRAST_COEFF = 10;
        private const double GAMMA_COEFF = 0.5;
        private const int KERNEL_SIZE = 9;
        
        private string? _path;
        private SystemBitmap? _originalImage;
        private SystemBitmap? _activeImage;

        #region functional filters
        
        private delegate int FunctionalFilter(int channel);
        private readonly FunctionalFilter _invertFilter = (channel) => 255 - channel;
        private readonly FunctionalFilter _brightenFilter = (channel) => 
        {
            var result = channel + BRIGHTNESS_COEFF;
            result = result > 255 ? 255 : result;
            result = result < 0 ? 0 : result;
            return result;
        };
        private readonly FunctionalFilter _contrastFilter = (channel) =>
        {
            var result = channel > 127 ? channel + CONTRAST_COEFF : channel - CONTRAST_COEFF;
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

        #endregion

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

        #region button clicks

        public async void SelectImage()
        {
            var filter = new FileDialogFilter() { Name = "Images", Extensions = {"svg", "jpg", "jpeg", "png"} };
            var dialog = new OpenFileDialog() { AllowMultiple = false, Filters = new List<FileDialogFilter>() {filter}};
            
            //TODO: change to normal check
            var desktop = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var window = desktop!.MainWindow;

            string[] result = await dialog.ShowAsync(window);
            Path = result?[0];

            OriginalImage = new AvaloniaBitmap(Path ?? string.Empty);
            ActiveImage = new AvaloniaBitmap(Path ?? string.Empty);
        }
        
        public async void InvertImage()
        {
            var pic = await ApplyFunctionalFilter(_activeImage!, _invertFilter);
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async void BrightenImage()
        {
            var pic = await ApplyFunctionalFilter(_activeImage!, _brightenFilter);
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async void ContrastImage()
        {
            var pic = await ApplyFunctionalFilter(_activeImage!, _contrastFilter);
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage)); 
        }

        public async void GammaImage()
        {
            var pic = await ApplyFunctionalFilter(_activeImage!, _gammaFilter);
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public void RestoreImage()
        {
            _activeImage = _originalImage;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public void BlurrImage()
        {
            _activeImage = _activeImage!.ApplyConvolutionalFilter<Blur>();
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async void SaveImage()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new() {Name = "JPG", Extensions = {"jpeg"}});
            dialog.Filters.Add(new() {Name = "PNG", Extensions = {"png"}});
            dialog.Filters.Add(new() {Name = "SVG", Extensions = {"svg"}});

            var desktop = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var window = desktop!.MainWindow;

            var result = await dialog.ShowAsync(window);

            if (result is not null)
            {
                _activeImage?.Save(result);
            }
        }
        
        #endregion
        
        #region image modifications

        private async Task<SystemBitmap> ApplyFunctionalFilter(SystemBitmap bitmap, FunctionalFilter filter)
        {
            var pic = new SystemBitmap(bitmap);
            
            await Task.Run(() =>
            {
                for (var y = 0; y <= pic.Height - 1; y++)
                {
                    for (var x = 0; x <= pic.Width - 1; x++)
                    {
                        var pixel = pic.GetPixel(x, y);
                        ApplyFunctionalFilterToPixel(ref pixel, filter);
                        pic.SetPixel(x, y, pixel);
                    }
                }
            });

            return pic;
        }

        private void ApplyFunctionalFilterToPixel(ref Color color, FunctionalFilter filter)
        {
            var red = filter(color.R);
            var green = filter(color.G);
            var blue = filter(color.B);
            color = Color.FromArgb(255, red, green, blue);
        }

        #endregion
    }
}