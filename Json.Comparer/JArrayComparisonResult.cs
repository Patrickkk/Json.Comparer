using System.Collections.Generic;

namespace Json.Comparer
{
    /// <summary>
    /// ComparisonResult for JArrayTokens
    /// </summary>
    public class JArrayComparisonResult : JTokenComparisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Array;

        /// <summary>
        /// The comparrisonResults for all elements in the array.
        /// </summary>
        public IEnumerable<JTokenComparisonResult> ArrayElementComparisons { get; set; } = new List<JTokenComparisonResult>();
    }
}