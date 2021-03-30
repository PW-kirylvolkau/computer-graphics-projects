namespace ComputerGraphics.Filters.Convolutional
{
    public class EdgeDetection : ConvolutionalFilter
    {
        public EdgeDetection()
        {
            CanNormalize = false;
        }
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