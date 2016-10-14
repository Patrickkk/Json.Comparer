using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Comparer.Filters;
using Newtonsoft.Json.Comparer.Tests.Filters;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests
{
    public class TestComparrisonFilters
    {
        [Fact]
        public void WhenfilteringAllNotDifferenceShouldReported()
        {
            var jobject = JToken.FromObject(new SimpleObject());
            var jobject2 = JToken.FromObject(new SimpleObject { Id = "12321", IntProperty = 214354 });

            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), new FilterAll()).Compare(jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Filtered, because: "I Filtered everything");
        }

        [Fact]
        public void WhenfilteringPropertyOnlyNonFilteredShouldReported()
        {
            var jobject1 = JObject.FromObject(new SimpleObject());
            var jobject2 = JObject.FromObject(new SimpleObject { IntProperty = 214354 });

            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), new FilterPropertyByName("IntProperty")).CompareObjects("root", jobject1, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different, because: "not everything should be filtered.");
            compareResult.PropertyComparrisons
                .Count(x => x.ComparrisonResult == ComparisonResult.Different)
                .Should().Be(0, because: "There is 1 different propertybut it is filtered.");
        }
    }
}