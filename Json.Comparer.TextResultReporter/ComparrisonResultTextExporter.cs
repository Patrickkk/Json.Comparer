using System;
using System.Collections.Generic;
using System.Linq;

namespace Json.Comparer.TextResultReporter
{
    public class ComparisonResultTextExporter
    {
        public static string Report(JTokenComparisonResult comparrisonResult, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (comparrisonResult != null)
            {
                return comparrisonResult.Match(
                    x => ReportObject(x, resultsToReport, settings).TrimEnd(Environment.NewLine.ToCharArray()),
                    x => ReportArray(x, resultsToReport, settings).TrimEnd(Environment.NewLine.ToCharArray()),
                    x => ReportProperty(x, resultsToReport, settings).TrimEnd(Environment.NewLine.ToCharArray()),
                    x => ReportValue(x, resultsToReport, settings).TrimEnd(Environment.NewLine.ToCharArray()));
            }
            else
            {
                return "";
            }
        }

        private static string ReportValue(JValueComparrisonResult valueComparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(valueComparrison.ComparisonResult))
            {
                return $"-{settings.Source1Name} value: '{valueComparrison.Source1Value?.ToString()}' - {settings.Source2Name} value:'{valueComparrison.Source2Value?.ToString()}'";
            }
            else
            {
                return "";
            }
        }

        private static string ReportProperty(JPropertyComparisonResult propertyComparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (propertyComparrison.PropertyValueComparisonResult?.Type == ComparedTokenType.Value)
            {
                return (ReportElement(propertyComparrison, settings)
                    + ReportValue((JValueComparrisonResult)propertyComparrison.PropertyValueComparisonResult, resultsToReport, settings)).TrimEnd(Environment.NewLine.ToCharArray());
            }
            else
            {
                return (ReportElement(propertyComparrison, settings)
                    + Environment.NewLine
                    + Report(propertyComparrison.PropertyValueComparisonResult, resultsToReport, settings)).TrimEnd(Environment.NewLine.ToCharArray());
            }
        }

        private static string ReportArray(JArrayComparrisonResult arrayComparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(arrayComparrison.ComparisonResult))
            {
                if (arrayComparrison.ComparisonResult == ComparisonResult.MissingInSource1 || arrayComparrison.ComparisonResult == ComparisonResult.MissingInSource2 || arrayComparrison.ComparisonResult == ComparisonResult.Identical)
                {
                    return ReportElement(arrayComparrison, settings);
                }
                var elementsToReport = arrayComparrison.ArrayElementComparrisons.Where(Comparison => resultsToReport.Contains(Comparison.ComparisonResult));

                return string.Join(Environment.NewLine, elementsToReport.Select(x => Report(x, resultsToReport, settings)).Where(x => !string.IsNullOrWhiteSpace(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportObject(JObjectComparisonResult objectcomparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(objectcomparrison.ComparisonResult))
            {
                if (objectcomparrison.ComparisonResult == ComparisonResult.MissingInSource1 || objectcomparrison.ComparisonResult == ComparisonResult.MissingInSource2 || objectcomparrison.ComparisonResult == ComparisonResult.Identical)
                {
                    return ReportElement(objectcomparrison, settings);
                }

                var propertiesToReport = objectcomparrison.PropertyComparisons.Where(propertyComparison => resultsToReport.Contains(propertyComparison.ComparisonResult));

                return ReportElement(objectcomparrison, settings)
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, propertiesToReport.Select(x => Report(x, resultsToReport, settings)).Where(x => !string.IsNullOrWhiteSpace(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportElement(JTokenComparisonResult result, ReporterSettings settings)
        {
            return $"{result.Path}-key:{result.Key}-{ComparrisonResultToFriendlyName(result, settings)}-{result.Type}";
        }

        private static string ComparrisonResultToFriendlyName(JTokenComparisonResult result, ReporterSettings settings)
        {
            switch (result.ComparisonResult)
            {
                case ComparisonResult.Filtered:
                case ComparisonResult.DifferentTypes:
                case ComparisonResult.Different:
                case ComparisonResult.Identical:
                    return result.ComparisonResult.ToString();

                case ComparisonResult.MissingInSource1:
                    return $"Missing in {settings.Source1Name}";

                case ComparisonResult.MissingInSource2:
                    return $"Missing in {settings.Source2Name}";

                default:
                    throw new ArgumentOutOfRangeException("ComparisonResult");
            }
        }
    }
}