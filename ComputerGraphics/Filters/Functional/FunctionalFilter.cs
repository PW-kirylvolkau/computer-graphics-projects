using ComputerGraphics.Filters.Interfaces;

namespace ComputerGraphics.Filters.Functional
{
    public abstract class FunctionalFilter
    {
        public delegate int FunctionalFilterDelegate(int channel);
        public FunctionalFilterDelegate Filter => GetFilter();

        protected abstract FunctionalFilterDelegate GetFilter();
    }
}