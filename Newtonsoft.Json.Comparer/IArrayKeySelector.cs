using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer
{
    public interface IArrayKeySelector
    {
        /// <summary>
        /// Select a key to be used for matching elements in an array. These must be unique.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        object SelectArrayKey(JToken token, int index);
    }
}
