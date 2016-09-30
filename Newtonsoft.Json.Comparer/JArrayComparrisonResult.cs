using System.Collections.Generic;

namespace Newtonsoft.Json.Comparer
{
    public class JArrayComparrisonResult : JTokenComparrisonResult
    {
        public override ComparisonResultType Type { get; set; } = ComparisonResultType.Array;

        public IEnumerable<JTokenComparrisonResult> ComparrisonResults { get; set; } = new List<JTokenComparrisonResult>();
    }
}