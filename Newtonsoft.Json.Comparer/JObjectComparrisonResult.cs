using System.Collections.Generic;

namespace Newtonsoft.Json.Comparer
{
    public class JObjectComparrisonResult : JTokenComparrisonResult
    {
        public override ComparisonResultType Type { get; set; } = ComparisonResultType.Object;

        public IEnumerable<JTokenComparrisonResult> PropertyComparrison { get; set; } = new List<JTokenComparrisonResult>();
    }
}