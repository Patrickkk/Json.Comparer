using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer
{
    public interface IComparrisonFilter
    {
        bool ShouldBeFiltered(string key, JToken token1, JToken token2);
    }
}