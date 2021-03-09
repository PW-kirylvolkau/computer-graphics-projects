namespace ComputerGraphics.Filters.Convolutional
{
    public class GaussianBlur : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            kernel = new double[,]
            {
                {0, 1, 0,},
                {1, 4, 1,},
                {0, 1, 0,},
            };

            return kernel;
        }
    }
}