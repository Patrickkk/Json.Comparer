using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NullGuard;

namespace Json.Comparer.ValueConverters
{
    public class NonConvertingConverter : IValueConverter
    {
        [return: AllowNull]
        public string Convert([AllowNull]string value)
        {
            return value;
        }
    }
}