using FluentAssertions;
using Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using System.Linq;
using Xunit;

namespace Json.Comparer.Tests
{
    public class TestMissingProperties
    {
        [Fact]
        public void ObjectWithPropertyMissingInSource2ShouldReportPropertyMissingInSource2()
        {
            var jobject = JToken.FromObject(new SimpleObjectWithExtraProperty());
            var jobject2 = JToken.FromObject(new SimpleObject());

            var compareResult = (JObjectComparisonResult)new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Different, because: "The JObjects are created from different type");
            compareResult.PropertyComparisons.First(x => x.Key == "ExtraIntProperty").ComparisonResult.Should().Be(ComparisonResult.MissingInSource2);
        }

        [Fact]
        public void ObjectWithPropertyMissingInSource2ShouldReportPropertyMissingInSource1()
        {
            var jobject = JToken.FromObject(new SimpleObject());
            var jobject2 = JToken.FromObject(new SimpleObjectWithExtraProperty());

            var compareResult = (JObjectComparisonResult)new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Different, because: "The JObjects are created from different type");
            compareResult.PropertyComparisons.First(x => x.Key == "ExtraIntProperty").ComparisonResult.Should().Be(ComparisonResult.MissingInSource1);
        }
    }
}