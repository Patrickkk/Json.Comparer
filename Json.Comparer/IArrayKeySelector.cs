using Newtonsoft.Json.Linq;

namespace Json.Comparer
{
    /// <summary>
    /// Defines the method used to match elements in an array. The standard supplied ArrayKeySelector will use indexes.
    /// </summary>
    public interface IArrayKeySelector
    {
        /// <summary>
        /// Select a key to be used for matching elements in an array. These must be unique.
        /// </summary>
        /// <param name="token">The element to compare</param>
        /// <param name="index">The index of the element</param>
        /// <returns></returns>
        object SelectArrayKey(JToken token, int index);
    }
}