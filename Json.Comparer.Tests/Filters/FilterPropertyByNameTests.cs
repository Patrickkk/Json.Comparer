﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Json.Comparer.Filters;
using Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Json.Comparer.Tests.Filters
{
    public class FilterPropertyByNameTests
    {
        [Fact]
        public void FilterPropertyByNameShouldFilterPropertyWithGivenName()
        {
            var jobject1 = JObject.FromObject(new SimpleObject());
            var jobject2 = JObject.FromObject(new SimpleObject { IntProperty = 214354, DateTimeProperty = new DateTime(2000, 10, 10), StringProperty = "dfdshjfg" });

            var filters = new List<IComparisonFilter>
            {
                new FilterPropertyByName("IntProperty"),
                new FilterPropertyByName("DateTimeProperty"),
            };
            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), filters).CompareObjects("root", jobject1, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Different, because: "not everything should be filtered.");
            compareResult.PropertyComparisons
                .Count(x => x.ComparisonResult == ComparisonResult.Different)
                .Should().Be(1, because: "1 changed property is filtered.");
        }

        [Fact]
        public void IfAllPropertiesAreFilteredTheObjectShouldBeIdentical()
        {
            var jobject1 = JObject.FromObject(new SimpleObject());
            var jobject2 = JObject.FromObject(new SimpleObject { IntProperty = 214354, DateTimeProperty = new DateTime(2000, 10, 10), StringProperty = "dfdshjfg" });

            var filters = new List<IComparisonFilter>
            {
                new FilterPropertyByName("IntProperty"),
                new FilterPropertyByName("DateTimeProperty"),
                new FilterPropertyByName("StringProperty"),
            };
            var compareResult = new JTokenComparer(new IndexArrayKeySelector(), filters).CompareObjects("root", jobject1, jobject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Identical, because: "not everything should be filtered.");
            compareResult.PropertyComparisons
                .Count(x => x.ComparisonResult == ComparisonResult.Different)
                .Should().Be(0, because: "All differences should be filtered.");
        }
    }
}