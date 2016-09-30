namespace Newtonsoft.Json.Comparer
{
    public class JPropertyComparrisonResult : JTokenComparrisonResult
    {
        public override ComparisonResultType Type { get; set; } = ComparisonResultType.Property;

        public string Name { get; set; }

        public JTokenComparrisonResult PropertyValueComparissonResult { get; set; }
    }
}