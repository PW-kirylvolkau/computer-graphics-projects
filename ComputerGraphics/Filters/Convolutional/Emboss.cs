namespace ComputerGraphics.Filters.Convolutional
{
    public class Emboss : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            // South - east emboss.
            kernel = new double[,]
            {
                {-1, -1, 0,},
                {-1, 1, 1,},
                {0, 1, 1,},
            };

            return kernel;
        }
    }
}