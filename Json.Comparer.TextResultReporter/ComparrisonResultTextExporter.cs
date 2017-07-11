using System;
using System.Collections.Generic;
using System.Linq;

namespace Json.Comparer.TextResultReporter
{
    public class ComparrisonResultTextExporter
    {
        public static string Report(JTokenComparrisonResult comparrisonResult, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
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
            if (resultsToReport.Contains(valueComparrison.ComparrisonResult))
            {
                return $"-{settings.Source1Name} value: '{valueComparrison.Source1Value?.ToString()}' - {settings.Source2Name} value:'{valueComparrison.Source2Value?.ToString()}'";
            }
            else
            {
                return "";
            }
        }

        private static string ReportProperty(JPropertyComparrisonResult propertyComparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (propertyComparrison.PropertyValueComparissonResult?.Type == ComparedTokenType.Value)
            {
                return (ReportElement(propertyComparrison, settings)
                    + ReportValue((JValueComparrisonResult)propertyComparrison.PropertyValueComparissonResult, resultsToReport, settings)).TrimEnd(Environment.NewLine.ToCharArray());
            }
            else
            {
                return (ReportElement(propertyComparrison, settings)
                    + Environment.NewLine
                    + Report(propertyComparrison.PropertyValueComparissonResult, resultsToReport, settings)).TrimEnd(Environment.NewLine.ToCharArray());
            }
        }

        private static string ReportArray(JArrayComparrisonResult arrayComparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(arrayComparrison.ComparrisonResult))
            {
                if (arrayComparrison.ComparrisonResult == ComparisonResult.MissingInSource1 || arrayComparrison.ComparrisonResult == ComparisonResult.MissingInSource2 || arrayComparrison.ComparrisonResult == ComparisonResult.Identical)
                {
                    return ReportElement(arrayComparrison, settings);
                }
                var elementsToReport = arrayComparrison.ArrayElementComparrisons.Where(comparrison => resultsToReport.Contains(comparrison.ComparrisonResult));

                return string.Join(Environment.NewLine, elementsToReport.Select(x => Report(x, resultsToReport, settings)).Where(x => !string.IsNullOrWhiteSpace(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportObject(JObjectComparrisonResult objectcomparrison, IEnumerable<ComparisonResult> resultsToReport, ReporterSettings settings)
        {
            if (resultsToReport.Contains(objectcomparrison.ComparrisonResult))
            {
                if (objectcomparrison.ComparrisonResult == ComparisonResult.MissingInSource1 || objectcomparrison.ComparrisonResult == ComparisonResult.MissingInSource2 || objectcomparrison.ComparrisonResult == ComparisonResult.Identical)
                {
                    return ReportElement(objectcomparrison, settings);
                }

                var propertiesToReport = objectcomparrison.PropertyComparrisons.Where(propertyComparrison => resultsToReport.Contains(propertyComparrison.ComparrisonResult));

                return ReportElement(objectcomparrison, settings)
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, propertiesToReport.Select(x => Report(x, resultsToReport, settings)).Where(x => !string.IsNullOrWhiteSpace(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportElement(JTokenComparrisonResult result, ReporterSettings settings)
        {
            return $"{result.Path}-key:{result.Key}-{ComparrisonResultToFriendlyName(result, settings)}-{result.Type}";
        }

        private static string ComparrisonResultToFriendlyName(JTokenComparrisonResult result, ReporterSettings settings)
        {
            switch (result.ComparrisonResult)
            {
                case ComparisonResult.Filtered:
                case ComparisonResult.DifferentTypes:
                case ComparisonResult.Different:
                case ComparisonResult.Identical:
                    return result.ComparrisonResult.ToString();

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