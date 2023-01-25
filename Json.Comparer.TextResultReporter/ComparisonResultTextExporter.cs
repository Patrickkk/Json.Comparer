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

        private static string ReportValue(JValueComparisonResult valueComparison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(valueComparison.ComparisonResult))
            {
                return $"-{settings.Source1Name} value: '{valueComparison.Source1Value?.ToString()}' - {settings.Source2Name} value:'{valueComparison.Source2Value?.ToString()}'";
            }
            else
            {
                return "";
            }
        }

        private static string ReportProperty(JPropertyComparisonResult propertyComparison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (propertyComparison.PropertyValueComparisonResult?.Type == ComparedTokenType.Value)
            {
                return (ReportElement(propertyComparison, settings)
                    + ReportValue((JValueComparisonResult)propertyComparison.PropertyValueComparisonResult, resultsToReport, settings)).TrimEnd(Environment.NewLine.ToCharArray());
            }
            else
            {
                return (ReportElement(propertyComparison, settings)
                    + Environment.NewLine
                    + Report(propertyComparison.PropertyValueComparisonResult, resultsToReport, settings)).TrimEnd(Environment.NewLine.ToCharArray());
            }
        }

        private static string ReportArray(JArrayComparisonResult arrayComparison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(arrayComparison.ComparisonResult))
            {
                if (arrayComparison.ComparisonResult == ComparisonResult.MissingInSource1 || arrayComparison.ComparisonResult == ComparisonResult.MissingInSource2 || arrayComparison.ComparisonResult == ComparisonResult.Identical)
                {
                    return ReportElement(arrayComparison, settings);
                }
                var elementsToReport = arrayComparison.ArrayElementComparisons.Where(Comparison => resultsToReport.Contains(Comparison.ComparisonResult));

                return string.Join(Environment.NewLine, elementsToReport.Select(x => Report(x, resultsToReport, settings)).Where(x => !string.IsNullOrWhiteSpace(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportObject(JObjectComparisonResult objectcomparison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(objectcomparison.ComparisonResult))
            {
                if (objectcomparison.ComparisonResult == ComparisonResult.MissingInSource1 || objectcomparison.ComparisonResult == ComparisonResult.MissingInSource2 || objectcomparison.ComparisonResult == ComparisonResult.Identical)
                {
                    return ReportElement(objectcomparison, settings);
                }

                var propertiesToReport = objectcomparison.PropertyComparisons.Where(propertyComparison => resultsToReport.Contains(propertyComparison.ComparisonResult));

                return ReportElement(objectcomparison, settings)
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
            return $"{result.Path}-key:{result.Key}-{ComparisonResultToFriendlyName(result, settings)}-{result.Type}";
        }

        private static string ComparisonResultToFriendlyName(JTokenComparisonResult result, ReporterSettings settings)
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