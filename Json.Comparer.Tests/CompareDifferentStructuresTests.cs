using System.Dynamic;
using System.Linq;
using FluentAssertions;
using Json.Comparer.ValueConverters;
using Xunit;

namespace Json.Comparer.Tests
{
    public class CompareDifferentStructuresTests
    {
        [Fact(Skip = "This actually doesn't work :(")]
        public void Compare2EqualObjectsShouldNotHaveDifference()
        {
            dynamic simpleObject1 = new ExpandoObject();
            simpleObject1.AAA = "";
            dynamic simpleObject2 = new ExpandoObject();
            simpleObject2.BBB = "";
            JTokenComparrisonResult compareResult = new JTokenComparer(new IndexArrayKeySelector(), Enumerable.Empty<IComparrisonFilter>(), new ComparisonResult[] { ComparisonResult.MissingInSource2 }, new NonConvertingConverter())
                .Compare(simpleObject1, simpleObject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }
    }
}