namespace ComputerGraphics.Filters.Convolutional
{
    public class Emboss : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            kernel = new double[,]
            {
                {2, 0, 0,},
                {0, -1, 0,},
                {0, 0, -1,},
            };

            return kernel;
        }
    }
}