using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ComputerGraphics.Filters.Convolutional;

namespace ComputerGraphics.Filters
{
    public static class BitmapFiltersExtensions
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
            var bufferLength = originalData.Stride * originalData.Height;
            byte[] pixelBuffer = new byte[bufferLength];
            byte[] resultBuffer = new byte[bufferLength];
            
            // We would like avoid overhead, created by the managed array to improve the performance, as convolutional
            // filters need a lot of operations to be performed. 
            // More on this matter:
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.copy?view=net-5.0
            Marshal.Copy(originalData.Scan0, pixelBuffer, 0, bufferLength);
            original.UnlockBits(originalData);

            var kernelSize = filter.KernelSize;
            var filterOffset = (kernelSize-1) / 2;

            for (var offsetY = filterOffset; offsetY < original.Height - filterOffset; offsetY++) 
            { 
                for (var offsetX = filterOffset; offsetX < original.Width - filterOffset; offsetX++)
                {
                    var blue = 0.0;
                    var green = 0.0;
                    var red = 0.0;

                    var byteOffset = offsetY * originalData.Stride + offsetX * 4; 
                    
                    for (var filterY = -filterOffset; filterY <= filterOffset; filterY++) 
                    {
                        for (var filterX = -filterOffset; filterX <= filterOffset; filterX++) 
                        {
                            var calcOffset = byteOffset + filterX * 4 + filterY * originalData.Stride;

                            var yOffset = filterY + filterOffset;
                            var xOffset = filterX + filterOffset;
                            var offsetKernel = filter.Kernel[yOffset, xOffset];
                            
                            blue += pixelBuffer[calcOffset] * offsetKernel;
                            green += pixelBuffer[calcOffset + 1] * offsetKernel;
                            red += pixelBuffer[calcOffset + 2] * offsetKernel; 
                        } 
                    }
                    
                    ClipChannelValue(ref blue);
                    ClipChannelValue(ref green);
                    ClipChannelValue(ref red);

                    resultBuffer[byteOffset] = (byte)blue; 
                    resultBuffer[byteOffset + 1] = (byte)green; 
                    resultBuffer[byteOffset + 2] = (byte)red; 
                    resultBuffer[byteOffset + 3] = 255; 
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
    }
}