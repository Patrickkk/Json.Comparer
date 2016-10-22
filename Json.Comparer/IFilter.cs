using Newtonsoft.Json.Linq;
using NullGuard;

namespace Json.Comparer
{
    public interface IComparrisonFilter
    {
        bool ShouldBeFiltered(string key, JToken token1, JToken token2);
    }
}