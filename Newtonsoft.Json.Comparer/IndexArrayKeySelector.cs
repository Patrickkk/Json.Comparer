using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer
{
    public class IndexArrayKeySelector : IArrayKeySelector
    {
        public object SelectArrayKey(JToken token, int index)
        {
            return index;
        }
    }
}
