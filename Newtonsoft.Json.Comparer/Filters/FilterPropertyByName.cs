using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer.Filters
{
    public class FilterPropertyByName : IComparrisonFilter
    {
        private readonly string propertyName;

        public FilterPropertyByName(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public bool ShouldBeFiltered(string key, JToken token1, JToken token2)
        {
            var type = token1.Type == JTokenType.Null ? token2.Type : token1.Type;
            if (type == JTokenType.Property)
            {
                var property = (JProperty)token1;
                return String.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}