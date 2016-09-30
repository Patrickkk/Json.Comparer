namespace Newtonsoft.Json.Comparer
{
    public class JTokenComparrisonResult
    {
        public string Key { get; set; }

        public virtual ComparisonResultType Type { get; set; }

        public ComparisonResult ComparrisonResult { get; set; }
    }
}