using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests
{
    public class TestMissingProperties
    {
        [Fact]
        public void ObjectWithPropertyMissingInSource2ShouldReportPropertyMissingInSource2()
        {
            var jobject = JToken.FromObject(new SimpleObjectWithExtraProperty());
            var jobject2 = JToken.FromObject(new SimpleObject());

            var compareResult = (JObjectComparrisonResult)new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different, because: "The JObjects are created from different type");
            compareResult.PropertyComparrisons.First(x => x.Key == "ExtraIntProperty").ComparrisonResult.Should().Be(ComparisonResult.MissingInSource2);
        }

        [Fact]
        public void ObjectWithPropertyMissingInSource2ShouldReportPropertyMissingInSource1()
        {
            var jobject = JToken.FromObject(new SimpleObject());
            var jobject2 = JToken.FromObject(new SimpleObjectWithExtraProperty());

            var compareResult = (JObjectComparrisonResult)new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different, because: "The JObjects are created from different type");
            compareResult.PropertyComparrisons.First(x => x.Key == "ExtraIntProperty").ComparrisonResult.Should().Be(ComparisonResult.MissingInSource1);
        }
    }
}