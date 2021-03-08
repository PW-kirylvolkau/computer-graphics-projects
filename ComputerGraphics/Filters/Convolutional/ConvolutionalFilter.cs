using ComputerGraphics.Filters.Interfaces;

namespace ComputerGraphics.Filters.Convolutional
{
    public abstract class ConvolutionalFilter : IConvolutionalFilter
    {
        public int KernelSize => 3;
        public double[,] Kernel => CalculateKernel();

        protected abstract double[,] CalculateKernel();
    }
}