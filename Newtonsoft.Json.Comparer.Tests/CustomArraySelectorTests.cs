using FluentAssertions;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests
{
    public class CustomArraySelectorTests
    {
        [Fact]
        public void CustomArraySelectorShouldUseIdToMatchElementsSoTheOrderShouldntMatter()
        {
            var child1 = new ComplexObject(false) { Id = "SomeId1" };
            var child2 = new ComplexObject(false) { Id = "SomeId2" };
            var child3 = new ComplexObject(false) { Id = "SomeId3" };

            var parent1 = new ComplexObject(false) { Children = new List<ComplexObject> { child1, child2, child3 } };
            var parent2 = new ComplexObject(false) { Children = new List<ComplexObject> { child3, child2, child1 } };

            var jobject = JToken.FromObject(parent1);
            var jobject2 = JToken.FromObject(parent2);

            var result = (JObjectComparrisonResult)new JTokenComparer(new CustomArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            result.ComparrisonResult.ShouldBeEquivalentTo(ComparisonResult.Identical);
        }

        [Fact]
        public void WhenArraySelectorResultsInDuplicateKeysItShouldThrowAnException()
        {
            var child1 = new ComplexObject(false) { Id = "SomeId1" };
            var child2 = new ComplexObject(false) { Id = "SomeId1" };
            var child3 = new ComplexObject(false) { Id = "SomeId1" };

            var parent1 = new ComplexObject(false) { Children = new List<ComplexObject> { child1, child2, child3 } };
            var parent2 = new ComplexObject(false) { Children = new List<ComplexObject> { child3, child2, child1 } };

            var jobject = JToken.FromObject(parent1);
            var jobject2 = JToken.FromObject(parent2);

            Assert.Throws<ArgumentOutOfRangeException>(() => (JObjectComparrisonResult)new JTokenComparer(new CustomArrayKeySelector()).CompareTokens("root", jobject, jobject2));
        }
    }
}