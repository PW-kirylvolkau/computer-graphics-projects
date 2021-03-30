namespace ComputerGraphics.Filters.Convolutional
{
    public class Sharpen : ConvolutionalFilter
    {
        public Sharpen()
        {
            CanNormalize = false;
        }
        protected override double[,] CalculateKernel()
        {
            // Mean removal.
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