namespace ComputerGraphics.Filters.Convolutional
{
    public class Identity : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            kernel = new double[,]
            {
                {0, 0, 0},
                {0, 1, 0},
                {0, 0, 0}
            };

            return kernel;
        }
    }
}