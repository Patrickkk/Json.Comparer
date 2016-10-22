using Newtonsoft.Json.Linq;

namespace Json.Comparer.Tests
{
    internal class CustomArrayKeySelector : IArrayKeySelector
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