using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using ComputerGraphics.Extensions;
using ComputerGraphics.Filters.Extensions;
using ComputerGraphics.Filters.Convolutional;
using ComputerGraphics.Filters.Functional;
using ReactiveUI;
using SystemBitmap = System.Drawing.Bitmap;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;
using SystemImage = System.Drawing.Image;

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

        #region image methods

        public async void SelectImage()
        {
            var filter = new FileDialogFilter() { Name = "Images", Extensions = {"svg", "jpg", "jpeg", "png"} };
            var dialog = new OpenFileDialog() { AllowMultiple = false, Filters = new List<FileDialogFilter>() {filter}};
            
            //TODO: change to normal check
            var desktop = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var window = desktop!.MainWindow;

            var result = await dialog.ShowAsync(window);
            var path = result.FirstOrDefault();
            if (path is not null)
            {
                OriginalImage = new AvaloniaBitmap(path);
                ActiveImage = new AvaloniaBitmap(path);
            }
        }
        
        public void RestoreImage()
        {
            _activeImage = _originalImage;
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

        #region functional filters

        public async void InvertImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyFunctionalFilter<Inversion>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async void BrightenImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyFunctionalFilter<Brightening>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        public async void ContrastImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyFunctionalFilter<Contrast>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage)); 
        }

        public async void GammaImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyFunctionalFilter<Gamma>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        #endregion

        #region convolutional filters

        public async void BlurImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<Blur>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public async void IdentityImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<Identity>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public async void EdgeDetection()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<EdgeDetection>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public async void Emboss()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<Emboss>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public async void GaussianBlur()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<GaussianBlur>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }
        
        public async void SharpenImage()
        {
            var pic = await Task.Run(() => _activeImage!.ApplyConvolutionalFilter<Sharpen>());
            _activeImage = pic;
            this.RaisePropertyChanged(nameof(ActiveImage));
        }

        #endregion
    }
}