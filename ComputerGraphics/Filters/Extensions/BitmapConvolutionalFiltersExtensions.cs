using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ComputerGraphics.Filters.Convolutional;

namespace ComputerGraphics.Filters.Extensions
{
    public static class BitmapConvolutionalFiltersExtensions
    {
        public static Bitmap ApplyConvolutionalFilter<T>(this Bitmap original)
            where T : ConvolutionalFilter, new()
        {
            var filter = new T();
            // Locks a bitmap to the system memory, allowing us to change its pixels values programmatically.
            // Offers better performance then SetPixel and GetPixel.
            // Source: https://docs.microsoft.com/en-us/dotnet/api/system.drawing.bitmap.lockbits?view=net-5.0
            var applianceRectangle = new Rectangle(0, 0, original.Width, original.Height);
            var lockMode = ImageLockMode.ReadOnly;
            var pixelFormat = PixelFormat.Format32bppArgb; // 32-bit per pixel (rgb - 24-bit, alpha - 8-bit).
            var originalData = original.LockBits(applianceRectangle, lockMode, pixelFormat);

            // Stride: https://docs.microsoft.com/pl-pl/dotnet/api/system.drawing.imaging.bitmapdata.stride?view=net-5.0
            var stride = originalData.Stride;
            var bufferLength = stride * originalData.Height;
            byte[] pixelBuffer = new byte[bufferLength];
            byte[] resultBuffer = new byte[bufferLength];
            
            // We would like avoid overhead, created by the managed array to improve the performance, as convolutional
            // filters need a lot of operations to be performed. 
            // More on this matter:
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.copy?view=net-5.0
            Marshal.Copy(originalData.Scan0, pixelBuffer, 0, bufferLength);
            original.UnlockBits(originalData);

            var kernel = filter.Kernel;
            var kernelSize = filter.KernelSize;
            var offset = (kernelSize-1) / 2;
            
            // rowIndex - row index of pixel.
            // columnIndex - column index of pixel.
            // offset - skipped pixels.
            for (var rowIndex = offset; rowIndex < original.Height - offset; rowIndex++) 
            { 
                for (var columnIndex = offset; columnIndex < original.Width - offset; columnIndex++)
                {
                    var blue = 0.0;
                    var green = 0.0;
                    var red = 0.0;

                    var pixelIndex = GetPixelIndex(rowIndex, stride, columnIndex);
                    
                    // iterate through neighbour pixels.
                    // relativeY, relativeX - offsetted position of the neighbour pixel
                    // with relation to the "main" pixel (available under pixelIndex).
                    for (var relativeY = -offset; relativeY <= offset; relativeY++) 
                    {
                        for (var relativeX = -offset; relativeX <= offset; relativeX++)
                        {
                            var distanceToNeighbour = GetPixelIndex(relativeY, stride, relativeX);
                            var updPixelIndex = pixelIndex + distanceToNeighbour; // index of pixel to be upd.

                            var yOffset = relativeY + offset;
                            var xOffset = relativeX + offset;
                            var coeff = kernel[yOffset, xOffset];
                            
                            blue += pixelBuffer[updPixelIndex] * coeff;
                            green += pixelBuffer[updPixelIndex + 1] * coeff;
                            red += pixelBuffer[updPixelIndex + 2] * coeff; 
                        } 
                    }
                    
                    ClipChannelValue(ref blue);
                    ClipChannelValue(ref green);
                    ClipChannelValue(ref red);

                    SetPixelChannels(resultBuffer, pixelIndex, red, green, blue);
                } 
            }

            // Reversing the process, done at the beginning of the method - instead of locking bits so we can
            // copy them more efficiently to buffer array, we lock them so we can write from the buffer array 
            // to the bits of resulting bitmap.
            var result = new Bitmap(original.Width, original.Height);
            var resultRectangle = new Rectangle(0, 0, result.Width, result.Height);
            var resultLockMode = ImageLockMode.WriteOnly;
            var resultData = result.LockBits(resultRectangle, resultLockMode, pixelFormat);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length); 
            result.UnlockBits(resultData);

            return result;
        }

        private static void ClipChannelValue(ref double value)
        {
            if (value > 255)
            {
                value = 255;
            }

            if (value < 0)
            {
                value = 0;
            }
        }

        private static int GetPixelIndex(int rowIndex, int stride, int columnIndex)
        {
            return rowIndex * stride + columnIndex * 4;
        }

        private static void SetPixelChannels(byte[] buffer, int index, double red, double green, double blue)
        {
            buffer[index] = (byte) blue; // blue channel
            buffer[index + 1] = (byte) green; // green channel
            buffer[index + 2] = (byte) red; // red channel
            buffer[index + 3] = 255; // alpha channel
        }
    }
}