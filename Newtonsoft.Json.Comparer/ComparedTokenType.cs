namespace Newtonsoft.Json.Comparer
{
    /// <summary>
    /// The type of JToken being compared. It is simplified representation of the JTokenType with a single Value for all different value types.
    /// </summary>
    public enum ComparedTokenType
    {
        /// <summary>
        /// The Tokesn compared consisted of Objects.
        /// </summary>
        Object = 0,

        /// <summary>
        /// The tokens compared consisted of Arrays.
        /// </summary>
        Array = 1,

        /// <summary>
        /// The tokens compared consisted of Properties.
        /// </summary>
        Property = 2,

        /// <summary>
        /// The tokens compared consisted of value type. So for example int, string datetime etc.
        /// </summary>
        Value = 3
    }
}