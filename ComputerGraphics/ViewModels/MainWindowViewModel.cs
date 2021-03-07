using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using ComputerGraphics.Extensions;
using ReactiveUI;
using SystemBitmap = System.Drawing.Bitmap;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;

namespace ComputerGraphics.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string? _path;
        private SystemBitmap? _originalImage;
        private SystemBitmap? _activeImage;

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

        public async void SelectImage()
        {
            var filter = new FileDialogFilter() { Name = "Images", Extensions = {"svg", "jpg", "jpeg", "png"} };
            var dialog = new OpenFileDialog() { AllowMultiple = false, Filters = new List<FileDialogFilter>() {filter}};

            var desktop = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var window = desktop!.MainWindow;

            string[] result = await dialog.ShowAsync(window);
            Path = result?[0];

            OriginalImage = new AvaloniaBitmap(Path ?? string.Empty);
            ActiveImage = new AvaloniaBitmap(Path ?? string.Empty);
        }

        public async void InvertImage()
        {
            //TODO: add conversion & data binding.
            if (_activeImage is not null)
            {
                var pic = await Invert(_activeImage);
                _activeImage = pic;
                this.RaisePropertyChanged(nameof(ActiveImage));
            }
        }

        private async Task<SystemBitmap> Invert(SystemBitmap bitmap)
        {
            var pic = new SystemBitmap(bitmap);
            
            await Task.Run(() =>
            {
                for (var y = 0; y <= pic.Height - 1; y++)
                {
                    for (var x = 0; x <= pic.Width - 1; x++)
                    {
                        var inv = pic.GetPixel(x, y);
                        inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                        pic.SetPixel(x, y, inv);
                    }
                }
            });

            return pic;
        }
    }
}
