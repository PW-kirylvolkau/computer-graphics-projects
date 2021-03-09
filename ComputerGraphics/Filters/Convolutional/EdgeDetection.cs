namespace ComputerGraphics.Filters.Convolutional
{
    public class EdgeDetection : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            kernel = new double[,] 
            {
                { -1, -1, -1, }, 
                { -1,  8, -1, },  
                { -1, -1, -1, }, 
            };
            return kernel;
        }
    }
}