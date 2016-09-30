using System;
using FluentAssertions;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests
{
    public class CompareDifferentObjects
    {
        [Fact]
        public void ObjectsWithDifferentValueForDateTimeShouldBeDifferent()
        {
            var jobject = JToken.FromObject(new SimpleObject { DateTimeProperty = new DateTime(2345, 1, 1) });
            var jobject2 = JToken.FromObject(new SimpleObject { DateTimeProperty = new DateTime(2000, 1, 1) });

            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different, because: "The JObjects are created with different datetime");
        }
    }
}