namespace ComputerGraphics.Filters.Convolutional
{
    public class EdgeDetection : ConvolutionalFilter
    {
        protected override double[,] CalculateKernel()
        {
            // Laplacian edge detection.
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