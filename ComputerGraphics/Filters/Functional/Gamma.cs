using System;

namespace ComputerGraphics.Filters.Functional
{
    public class Gamma : FunctionalFilter
    {
        private const double GAMMA_COEFF = 0.5;
        
        private readonly FunctionalFilterDelegate _gammaFilter = (channel) =>
        {
            var gammaCorrection = 1 / GAMMA_COEFF;
            var tmp = (double) channel / 255;
            var result = 255 * Math.Pow(tmp, gammaCorrection);
            result = result > 255 ? 255 : result;
            return (int) result;
        };
        protected override FunctionalFilterDelegate GetFilter()
        {
            return _gammaFilter;
        }
    }
}