using NullGuard;

namespace Json.Comparer
{
    /// <summary>
    /// The result of a JToken comparison.
    /// </summary>
    public class JPropertyComparrisonResult : JTokenComparrisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Property;

        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The comparrisonresult of the value of the property.
        /// </summary>
        [AllowNull]
        public JTokenComparrisonResult PropertyValueComparissonResult { get; set; }
    }
}