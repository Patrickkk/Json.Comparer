using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Comparer.TextResultReporter;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests
{
    public class ComplexObjectDifferenceTests
    {
        [Fact]
        public void ComparrisonWithMissingChildrenShouldReportDifference()
        {
            var complexObject = new ComplexObject(true);
            complexObject.DateTimeProperty = DateTime.UtcNow;
            var complexObject2 = new ComplexObject(false);
            var jobject = JToken.FromObject(complexObject);
            var jobject2 = JToken.FromObject(complexObject2);

            var compareResult = (JObjectComparrisonResult)new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different);
            var childrenComparrison = (JPropertyComparrisonResult)compareResult.PropertyComparrisons.First(x => x.Key == "Children");
            var childrenValueComparrison = (JArrayComparrisonResult)childrenComparrison.PropertyValueComparissonResult;
            childrenValueComparrison.ComparrisonResult.Should().Be(ComparisonResult.Different);
            childrenValueComparrison.ArrayElementComparrisons.All(x => x.ComparrisonResult == ComparisonResult.MissingInSource2).Should().Be(true);

            var report = ComparrisonResultTextExporter.Report(compareResult, new ComparisonResult[] { ComparisonResult.Different, ComparisonResult.MissingInSource1, ComparisonResult.MissingInSource2 });

            var compareResultAsJson = JsonConvert.SerializeObject(compareResult, Formatting.Indented);
        }

        [Fact]
        public void ComparrisonWithComplexObjectShouldCorrectlyReportDifference()
        {
            throw new NotImplementedException();
        }
    }
}