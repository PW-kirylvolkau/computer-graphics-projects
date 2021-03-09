using System;
using ComputerGraphics.Filters.Interfaces;

namespace ComputerGraphics.Filters.Functional
{
    public class Inversion : FunctionalFilter
    {
        private readonly FunctionalFilterDelegate _invertFilter = (channel) => 255 - channel;
        protected override FunctionalFilterDelegate GetFilter()
        {
            return _invertFilter;
        }
    }
}