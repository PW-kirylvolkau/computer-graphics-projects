using ComputerGraphics.Filters.Interfaces;

namespace ComputerGraphics.Filters.Convolutional
{
    public abstract class ConvolutionalFilter : IConvolutionalFilter
    {
        protected double[,]? kernel;
        public int KernelSize => 3; // however, returning Kernel.Length is more optimal.
        public bool CanNormalize { get; set; }
        public double[,] Kernel => kernel ?? CalculateKernel(); 
        protected abstract double[,] CalculateKernel();
    }
}