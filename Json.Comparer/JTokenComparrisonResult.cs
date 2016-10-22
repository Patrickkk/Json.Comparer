namespace Json.Comparer
{
    /// <summary>
    /// The result of a comparison.
    /// </summary>
    public class JTokenComparrisonResult
    {
        public string Key { get; set; }

        public string Path { get; set; }

        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public virtual ComparedTokenType Type { get; }

        /// <summary>
        /// The result of the comparrison. Different if the token or any of the child elements are different.
        /// </summary>
        public ComparisonResult ComparrisonResult { get; set; }
    }
}