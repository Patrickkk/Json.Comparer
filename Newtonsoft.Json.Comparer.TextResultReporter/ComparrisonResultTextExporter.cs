using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Comparer.TextResultReporter
{
    public class ComparrisonResultTextExporter
    {
        public static string Report(JTokenComparrisonResult comparrisonResult, IEnumerable<ComparisonResult> resultsToReport, int indent = 0)
        {
            return comparrisonResult.Match(
                x => ReportObject(x, resultsToReport, indent),
                x => ReportArray(x, resultsToReport, indent),
                x => ReportProperty(x, resultsToReport, indent),
                x => ReportValue(x, resultsToReport, indent));
        }

        private static string ReportValue(JValueComparrisonResult valueComparrison, IEnumerable<ComparisonResult> resultsToReport, int indent = 0)
        {
            if (resultsToReport.Contains(valueComparrison.ComparrisonResult))
            {
                return WithIndent($"{valueComparrison.ComparrisonResult} - source1Value '{valueComparrison.Source1Value?.ToString()}' - source2Value '{valueComparrison.Source2Value?.ToString()}'", indent);
            }
            else
            {
                return "";
            }
        }

        private static string ReportProperty(JPropertyComparrisonResult propertyComparrison, IEnumerable<ComparisonResult> resultsToReport, int indent = 0)
        {
            if (propertyComparrison.PropertyValueComparissonResult.Type == ComparedTokenType.Value)
            {

                return WithIndent($"{propertyComparrison.Key} - {ReportValue((JValueComparrisonResult)propertyComparrison.PropertyValueComparissonResult, resultsToReport)}", indent);
            }
            else
            {
                return WithIndent($"{propertyComparrison.Key} - {propertyComparrison.ComparrisonResult} - {propertyComparrison.PropertyValueComparissonResult.Type}", indent) + Environment.NewLine
                    + Report(propertyComparrison.PropertyValueComparissonResult, resultsToReport, indent + 1);
            }
        }

        private static string ReportArray(JArrayComparrisonResult arrayComparrison, IEnumerable<ComparisonResult> resultsToReport, int indent = 0)
        {
            if (resultsToReport.Contains(arrayComparrison.ComparrisonResult))
            {
                if (arrayComparrison.ComparrisonResult == ComparisonResult.MissingInSource1 || arrayComparrison.ComparrisonResult == ComparisonResult.MissingInSource2)
                {
                    return WithIndent($"{arrayComparrison.Key} - {arrayComparrison.ComparrisonResult} - array", indent);
                }
                var elementsToReport = arrayComparrison.ArrayElementComparrisons.Where(comparrison => resultsToReport.Contains(comparrison.ComparrisonResult));

                return string.Join(Environment.NewLine, elementsToReport.Select(x => Report(x, resultsToReport, indent)).Where(x => !string.IsNullOrEmpty(x)));
            }
            else
            {
                return "";
            }
        }

        private static string ReportObject(JObjectComparrisonResult objectcomparrison, IEnumerable<ComparisonResult> resultsToReport, int indent = 0)
        {
            if (resultsToReport.Contains(objectcomparrison.ComparrisonResult))
            {
                if (objectcomparrison.ComparrisonResult == ComparisonResult.MissingInSource1 || objectcomparrison.ComparrisonResult == ComparisonResult.MissingInSource2)
                {
                    return WithIndent($"{objectcomparrison.Key} - {objectcomparrison.ComparrisonResult} - object ", indent);
                }

                var propertiesToReport = objectcomparrison.PropertyComparrisons.Where(propertyComparrison => resultsToReport.Contains(propertyComparrison.ComparrisonResult));

                return WithIndent($"{objectcomparrison.Key} - {objectcomparrison.ComparrisonResult} - object", indent) + Environment.NewLine
                    + string.Join(Environment.NewLine, propertiesToReport.Select(x => Report(x, resultsToReport, indent + 1)).Where(x => !string.IsNullOrEmpty(x)));
            }
            else
            {
                return "";
            }
        }

        private static string WithIndent(string text, int indent = 0)
        {
            return "".PadLeft(indent, '-') + " " + text;
        }
    }
}