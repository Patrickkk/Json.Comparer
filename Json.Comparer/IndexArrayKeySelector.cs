using Newtonsoft.Json.Linq;

namespace Json.Comparer
{
    /// <summary>
    /// The key selector for comparing arrays based on the index of the element in the collection.
    /// </summary>
    public class IndexArrayKeySelector : IArrayKeySelector
    {
        /// <summary>
        /// Select a key to be used for matching elements in an array. These must be unique.
        /// </summary>
        /// <param name="token">The element to compare</param>
        /// <param name="index">The index of the element</param>
        /// <returns></returns>
        public object SelectArrayKey(JToken token, int index)
        {
            return index;
        }
    }
}