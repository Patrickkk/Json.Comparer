using System.Collections.Generic;

namespace Json.Comparer
{
    /// <summary>
    /// ComparisonResult for JArrayTokens
    /// </summary>
    public class JArrayComparrisonResult : JTokenComparisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Array;

        /// <summary>
        /// The comparrisonResults for all elements in the array.
        /// </summary>
        public IEnumerable<JTokenComparisonResult> ArrayElementComparrisons { get; set; } = new List<JTokenComparisonResult>();
    }
}