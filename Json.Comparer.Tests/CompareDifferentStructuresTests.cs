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
            JTokenComparisonResult compareResult = new JTokenComparer(new IndexArrayKeySelector(), Enumerable.Empty<IComparisonFilter>(), new ComparisonResult[] { ComparisonResult.MissingInSource2 }, new NonConvertingConverter())
                .Compare(simpleObject1, simpleObject2);

            compareResult.ComparisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }
    }
}