using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace Json.Comparer.Filters
{
    public class FilterPropertyByName : IComparisonFilter
    {
        private readonly string propertyName;

        public FilterPropertyByName(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public bool ShouldBeFiltered(string key, [AllowNull]JToken token1, [AllowNull]JToken token2)
        {
            var type = GetTokenType(token1, token2);
            if (type == JTokenType.Property)
            {
                var property = (JProperty)FirstNonNullValue(token1, token2);
                return String.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static JTokenType GetTokenType(JToken token1, JToken token2)
        {
            if (token1?.Type == null || token1?.Type == JTokenType.Null)
            {
                return token2.Type;
            }
            else
            {
                return token1.Type;
            }
        }

        private static T FirstNonNullValue<T>(params T[] values)
    where T : class
        {
            return values.First(x => x != null);
        }

        private static T FirstNonNullValueOrDefault<T>(T defaultValue, params T[] values)
            where T : class
        {
            var value = values.FirstOrDefault(x => x != null);
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }
    }
}