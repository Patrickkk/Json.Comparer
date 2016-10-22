using FluentAssertions;
using Json.Comparer.Tests.TestObjects;
using System;
using Xunit;

namespace Json.Comparer.Tests
{
    public class CompareDifferentObjects
    {
        [Fact]
        public void ObjectsWithDifferentValueForDateTimeShouldBeDifferent()
        {
            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).Compare(
                new SimpleObject { DateTimeProperty = new DateTime(2345, 1, 1) },
                new SimpleObject { DateTimeProperty = new DateTime(2000, 1, 1) });

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Different, because: "The JObjects are created with different datetime");
        }
    }
}