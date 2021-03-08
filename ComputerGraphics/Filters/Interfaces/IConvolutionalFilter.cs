namespace ComputerGraphics.Filters.Interfaces
{
    public interface IConvolutionalFilter
    {
        public int KernelSize { get; }
        public double[,] Kernel { get; }
    }
}