using System.Collections.Generic;

namespace Newtonsoft.Json.Comparer
{
    /// <summary>
    /// ComparisonResult for JArrayTokens
    /// </summary>
    public class JArrayComparrisonResult : JTokenComparrisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Array;

        /// <summary>
        /// The comparrisonResults for all elements in the array.
        /// </summary>
        public IEnumerable<JTokenComparrisonResult> ArrayElementComparrisons { get; set; } = new List<JTokenComparrisonResult>();
    }
}