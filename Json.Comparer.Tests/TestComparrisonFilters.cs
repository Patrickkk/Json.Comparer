using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Json.Comparer.Filters;
using Json.Comparer.Tests.Filters;
using Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Json.Comparer.Tests
{
    public class TestComparrisonFilters
    {
        [Fact]
        public void WhenfilteringAllNotDifferenceShouldReported()
        {
            var jobject = JToken.FromObject(new SimpleObject());
            var jobject2 = JToken.FromObject(new SimpleObject { Id = "12321", IntProperty = 214354 });

            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), new FilterAll()).Compare(jobject, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Filtered, because: "I Filtered everything");
        }

        [Fact]
        public void WhenfilteringPropertyOnlyNonFilteredShouldReported()
        {
            var jobject1 = JObject.FromObject(new SimpleObject());
            var jobject2 = JObject.FromObject(new SimpleObject { IntProperty = 214354 });

            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), new FilterPropertyByName("IntProperty")).CompareObjects("root", jobject1, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Identical, because: "Every different property is filtered.");
            compareResult.PropertyComparisons
                .Count(x => x.ComparisonResult == ComparisonResult.Different)
                .Should().Be(0, because: "There is 1 different propertybut it is filtered.");
        }
    }
}