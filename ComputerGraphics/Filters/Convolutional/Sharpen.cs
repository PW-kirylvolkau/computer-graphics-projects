namespace ComputerGraphics.Filters.Convolutional
{
    public class Sharpen : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            kernel = new double[,]
            {
                {-1, -1, -1,},
                {-1, 9, -1,},
                {-1, -1, -1,}
            };

            return kernel;
        }
    }
}