using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Json.Comparer.Tests.Filters
{
    internal class FilterAll : IComparrisonFilter
    {
        public bool ShouldBeFiltered(string key, JToken token1, JToken token2)
        {
            return true;
        }
    }
}