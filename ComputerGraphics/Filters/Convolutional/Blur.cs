namespace ComputerGraphics.Filters.Convolutional
{
    public class Blur : ConvolutionalFilter
    {
        private const double BLUR_COEFF = 0.2;
        protected override double[,] CalculateKernel()
        {
            kernel = new double[KernelSize, KernelSize];
            
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