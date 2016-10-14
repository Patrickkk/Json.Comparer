namespace Newtonsoft.Json.Comparer
{
    /// <summary>
    /// The result of a comparrison.
    /// </summary>
    public enum ComparisonResult
    {
        /// <summary>
        /// The values contained in this token and any child tokens are identical
        /// </summary>
        Identical,

        /// <summary>
        /// The value in this token or values in child elements are different, or missing.
        /// </summary>
        Different,

        /// <summary>
        /// The element is missing in source1.
        /// </summary>
        MissingInSource1,

        /// <summary>
        /// The element is missing in source2.
        /// </summary>
        MissingInSource2,

        /// <summary>
        /// The element is of a different type in source and target. This indicates there is an incorrect comparrison.
        /// </summary>
        DifferentTypes,

        /// <summary>
        /// The tokens comparrison was skipped and filtered out.
        /// </summary>
        Filtered,
    }
}