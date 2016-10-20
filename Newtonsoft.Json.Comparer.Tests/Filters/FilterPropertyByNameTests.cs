using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Comparer.Filters;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests.Filters
{
    public class FilterPropertyByNameTests
    {
        [Fact]
        public void FilterPropertyByNameShouldFilterPropertyWithGivenName()
        {
            var jobject1 = JObject.FromObject(new SimpleObject());
            var jobject2 = JObject.FromObject(new SimpleObject { IntProperty = 214354, DateTimeProperty = new DateTime(2000, 10, 10), StringProperty = "dfdshjfg" });

            var filters = new List<IComparrisonFilter>
            {
                new FilterPropertyByName("IntProperty"),
                new FilterPropertyByName("DateTimeProperty"),
            };
            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), filters).CompareObjects("root", jobject1, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different, because: "not everything should be filtered.");
            compareResult.PropertyComparrisons
                .Count(x => x.ComparrisonResult == ComparisonResult.Different)
                .Should().Be(1, because: "1 changed property is filtered.");
        }

        [Fact]
        public void IfAllPropertiesAreFilteredTheObjectShouldBeIdentical()
        {
            var jobject1 = JObject.FromObject(new SimpleObject());
            var jobject2 = JObject.FromObject(new SimpleObject { IntProperty = 214354, DateTimeProperty = new DateTime(2000, 10, 10), StringProperty = "dfdshjfg" });

            var filters = new List<IComparrisonFilter>
            {
                new FilterPropertyByName("IntProperty"),
                new FilterPropertyByName("DateTimeProperty"),
                new FilterPropertyByName("StringProperty"),
            };
            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), filters).CompareObjects("root", jobject1, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "not everything should be filtered.");
            compareResult.PropertyComparrisons
                .Count(x => x.ComparrisonResult == ComparisonResult.Different)
                .Should().Be(0, because: "All differences should be filtered.");
        }
    }
}