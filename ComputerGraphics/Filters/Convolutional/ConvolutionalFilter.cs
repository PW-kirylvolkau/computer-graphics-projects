using ComputerGraphics.Filters.Interfaces;

namespace ComputerGraphics.Filters.Convolutional
{
    public abstract class ConvolutionalFilter : IConvolutionalFilter
    {
        protected double[,]? kernel;
        public int KernelSize => 3;
        public double[,] Kernel => kernel ?? CalculateKernel(); 
        protected abstract double[,] CalculateKernel();
    }
}