using ComputerGraphics.Filters.Interfaces;

namespace ComputerGraphics.Filters.Convolutional
{
    public class Blur : ConvolutionalFilter
    {
        private const double BLUR_COEFF = 1;
        protected override double[,] CalculateKernel()
        {
            var kernel = new double[KernelSize, KernelSize];

            for (var i = 0; i < KernelSize; i++)
            {
                for (var j = 0; j < KernelSize; j++)
                {
                    kernel[i,j] = BLUR_COEFF;
                }
            }

            return kernel;
        }
    }
}