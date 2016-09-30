using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer.Tests
{
    class CustomArrayKeySelector : IArrayKeySelector
    {
        public object SelectArrayKey(JToken token, int index)
        {
            if (token.Type == JTokenType.Object)
            {
                var objectToken = (JObject)token;
                return objectToken["Id"];
            }
            else
            {
                return index;
            }
        }
    }
}
