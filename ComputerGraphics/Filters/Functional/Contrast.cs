namespace ComputerGraphics.Filters.Functional
{
    public class Contrast : FunctionalFilter
    {
        private const int CONTRAST_COEFF = 10; 
        
        private readonly FunctionalFilterDelegate _contrastFilter = (channel) =>
        {
            var result = channel > 127 ? channel + CONTRAST_COEFF : channel - CONTRAST_COEFF;
            result = result > 255 ? 255 : result;
            result = result < 0 ? 0 : result;
            return result;
        };
        
        protected override FunctionalFilterDelegate GetFilter()
        {
            return _contrastFilter;
        }
    }
}