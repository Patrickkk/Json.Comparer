using FluentAssertions;
using Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Json.Comparer.Tests
{
    public class CompareEqualObjects
    {
        [Fact]
        public void Compare2EqualObjectsShouldNotHaveDifference()
        {
            var simpleObject = new SimpleObject();
            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).Compare(simpleObject, simpleObject);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }

        [Fact]
        public void Compare2EqualArraysShouldHaveNoDifference()
        {
            var array = new SimpleObject[] {
                new SimpleObject(),
                new SimpleObject(),
                new SimpleObject(),
            };

            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).Compare(array, array);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }

        [Fact]
        public void Comparrison2EqualComplextObjectsShouldHaveNoDifference()
        {
            var complexObject = new ComplexObject(true);
            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).Compare(complexObject, complexObject);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }
    }
}