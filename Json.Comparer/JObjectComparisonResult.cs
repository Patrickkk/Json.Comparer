using System.Collections.Generic;

namespace Json.Comparer
{
    /// <summary>
    /// A ComparisonResult for JObjectToken elements.
    /// </summary>
    public class JObjectComparisonResult : JTokenComparisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Object;

        /// <summary>
        /// The comparrisonResults for all properties in the object.
        /// </summary>
        public IEnumerable<JTokenComparisonResult> PropertyComparisons { get; set; } = new List<JTokenComparisonResult>();
    }
}