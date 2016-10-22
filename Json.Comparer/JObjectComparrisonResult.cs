using System.Collections.Generic;

namespace Json.Comparer
{
    /// <summary>
    /// A ComparisonResult for JObjectToken elements.
    /// </summary>
    public class JObjectComparrisonResult : JTokenComparrisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Object;

        /// <summary>
        /// The comparrisonResults for all properties in the object.
        /// </summary>
        public IEnumerable<JTokenComparrisonResult> PropertyComparrisons { get; set; } = new List<JTokenComparrisonResult>();
    }
}