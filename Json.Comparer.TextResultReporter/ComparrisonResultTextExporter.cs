using System;
using System.Collections.Generic;
using System.Linq;

namespace Json.Comparer.TextResultReporter
{
    public class ComparrisonResultTextExporter
    {
        public static string Report(JTokenComparrisonResult comparrisonResult, IEnumerable<ComparisonResult> resultsToReport)
        {
            if (comparrisonResult != null)
            {
                return comparrisonResult.Match(
                    x => ReportObject(x, resultsToReport),
                    x => ReportArray(x, resultsToReport),
                    x => ReportProperty(x, resultsToReport),
                    x => ReportValue(x, resultsToReport));
            }
            else
            {
                return "";
            }
        }

        private static string ReportValue(JValueComparrisonResult valueComparrison, IEnumerable<ComparisonResult> resultsToReport)
        {
            if (resultsToReport.Contains(valueComparrison.ComparrisonResult))
            {
                return $"-source1Value '{valueComparrison.Source1Value?.ToString()}' - source2Value '{valueComparrison.Source2Value?.ToString()}'";
            }
            else
            {
                return "";
            }
        }

        private static string ReportProperty(JPropertyComparrisonResult propertyComparrison, IEnumerable<ComparisonResult> resultsToReport)
        {
            if (propertyComparrison.PropertyValueComparissonResult?.Type == ComparedTokenType.Value)
            {
                return ReportElement(propertyComparrison)
                    + ReportValue((JValueComparrisonResult)propertyComparrison.PropertyValueComparissonResult, resultsToReport);
            }
            else
            {
                return ReportElement(propertyComparrison)
                    + Environment.NewLine
                    + Report(propertyComparrison.PropertyValueComparissonResult, resultsToReport);
            }
        }

        private static string ReportArray(JArrayComparrisonResult arrayComparrison, IEnumerable<ComparisonResult> resultsToReport)
        {
            if (resultsToReport.Contains(arrayComparrison.ComparrisonResult))
            {
                if (arrayComparrison.ComparrisonResult == ComparisonResult.MissingInSource1 || arrayComparrison.ComparrisonResult == ComparisonResult.MissingInSource2)
                {
                    return ReportElement(arrayComparrison);
                }
                var elementsToReport = arrayComparrison.ArrayElementComparrisons.Where(comparrison => resultsToReport.Contains(comparrison.ComparrisonResult));

                return string.Join(Environment.NewLine, elementsToReport.Select(x => Report(x, resultsToReport)).Where(x => !string.IsNullOrEmpty(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportObject(JObjectComparrisonResult objectcomparrison, IEnumerable<ComparisonResult> resultsToReport)
        {
            if (resultsToReport.Contains(objectcomparrison.ComparrisonResult))
            {
                if (objectcomparrison.ComparrisonResult == ComparisonResult.MissingInSource1 || objectcomparrison.ComparrisonResult == ComparisonResult.MissingInSource2)
                {
                    return ReportElement(objectcomparrison);
                }

                var propertiesToReport = objectcomparrison.PropertyComparrisons.Where(propertyComparrison => resultsToReport.Contains(propertyComparrison.ComparrisonResult));

                return ReportElement(objectcomparrison)
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, propertiesToReport.Select(x => Report(x, resultsToReport)).Where(x => !string.IsNullOrEmpty(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportElement(JTokenComparrisonResult result)
        {
            return $"{result.Path}-key:{result.Key}-{result.ComparrisonResult.ToString()}-{result.Type}";
        }
    }
}