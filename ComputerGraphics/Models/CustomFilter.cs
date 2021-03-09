using System.Collections.Generic;
using ComputerGraphics.Filters.Functional;

namespace ComputerGraphics.Models
{
    public class CustomFilter
    {
        public string? Name { get; set; }
        public List<FunctionalFilter.FunctionalFilterDelegate> FunctionalFilters;

        public CustomFilter()
        {
            FunctionalFilters = new List<FunctionalFilter.FunctionalFilterDelegate>();
        }
    }
}