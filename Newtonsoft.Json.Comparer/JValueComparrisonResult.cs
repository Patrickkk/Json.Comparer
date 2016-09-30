namespace Newtonsoft.Json.Comparer
{
    public class JValueComparrisonResult : JTokenComparrisonResult
    {
        public override ComparisonResultType Type { get; set; } = ComparisonResultType.Value;

        public string Source1Value { get; set; }

        public string Source2Value { get; set; }
    }
}