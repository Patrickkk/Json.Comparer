using System;
using System.Linq;
using FluentAssertions;
using Json.Comparer.Tests.TestObjects;
using Json.Comparer.TextResultReporter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Json.Comparer.Tests
{
    public class ComplexObjectDifferenceTests
    {
        [Fact]
        public void ComparisonWithMissingChildrenShouldReportDifference()
        {
            var complexObject = new ComplexObject(true)
            {
                DateTimeProperty = DateTime.UtcNow
            };

            var complexObject2 = new ComplexObject(false);
            var jobject = JToken.FromObject(complexObject);
            var jobject2 = JToken.FromObject(complexObject2);

            var compareResult = (JObjectComparisonResult)new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Different);
            var childrenComparison = (JPropertyComparisonResult)compareResult.PropertyComparisons.First(x => x.Key == "Children");
            var childrenValueComparison = (JArrayComparisonResult)childrenComparison.PropertyValueComparisonResult;
            childrenValueComparison.ComparisonResult.Should().Be(ComparisonResult.Different);
            childrenValueComparison.ArrayElementComparisons.All(x => x.ComparisonResult == ComparisonResult.MissingInSource2).Should().Be(true);

            var report = ComparisonResultTextExporter.Report(compareResult, new ComparisonResult[] { ComparisonResult.Different, ComparisonResult.MissingInSource1, ComparisonResult.MissingInSource2 }, new ReporterSettings { Source1Name = "QA", Source2Name = "PROD" });

            var compareResultAsJson = JsonConvert.SerializeObject(compareResult, Formatting.Indented);
        }

        [Fact(Skip = "TODO")]
        public void ComparisonWithComplexObjectShouldCorrectlyReportDifference()
        {
            throw new NotImplementedException();
        }
    }
}