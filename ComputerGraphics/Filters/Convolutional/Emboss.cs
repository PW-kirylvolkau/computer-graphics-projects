namespace ComputerGraphics.Filters.Convolutional
{
    public class Emboss : ConvolutionalFilter
    {
        public Emboss()
        {
            CanNormalize = false;
        }
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