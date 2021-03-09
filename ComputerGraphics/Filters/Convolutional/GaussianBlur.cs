namespace ComputerGraphics.Filters.Convolutional
{
    public class GaussianBlur : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            kernel = new double[,]
            {
                {1, 2, 1,},
                {2, 4, 2,},
                {1, 2, 1,},
            };

            return kernel;
        }
    }
}