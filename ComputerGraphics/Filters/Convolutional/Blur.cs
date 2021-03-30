namespace ComputerGraphics.Filters.Convolutional
{
    public class Blur : ConvolutionalFilter
    {
        public Blur()
        {
            CanNormalize = true;
        }
        protected override double[,] CalculateKernel()
        {
            
            kernel = new double[,]
            {
                {0.0, 0.2, 0.0,},
                {0.2, 0.2, 0.2,},
                {0.0, 0.2, 0.2,},
            };

            return kernel;
        }
    }
}