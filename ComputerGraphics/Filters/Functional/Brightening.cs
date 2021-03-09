namespace ComputerGraphics.Filters.Functional
{
    public class Brightening : FunctionalFilter
    {
        private const int BRIGHTNESS_COEFF = 10;
        
        private readonly FunctionalFilterDelegate _brightenFilter = (channel) => 
        {
            var result = channel + BRIGHTNESS_COEFF;
            result = result > 255 ? 255 : result;
            result = result < 0 ? 0 : result;
            return result;
        };
        
        protected override FunctionalFilterDelegate GetFilter()
        {
            return _brightenFilter;
        }
    }
}