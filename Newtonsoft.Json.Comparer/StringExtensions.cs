using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Comparer
{
    public static class StringExtensions
    {
        public static string EmptyIfNull(this string value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value;
            }
        }
    }
}