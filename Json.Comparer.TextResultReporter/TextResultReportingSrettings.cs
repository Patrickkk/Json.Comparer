using System.Collections.Generic;

namespace Json.Comparer.TextResultReporter
{
    public class TextResultReportingSettings
    {
        public IEnumerable<ComparisonResult> resultsToReport { get; set; } = new HashSet<ComparisonResult>();

        public IEnumerable<ComparisonResult> rootResultsToReport { get; set; } = new HashSet<ComparisonResult>();
    }
}