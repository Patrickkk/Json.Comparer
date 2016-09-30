using FluentAssertions;
using Newtonsoft.Json.Comparer.Tests.TestObjects;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Newtonsoft.Json.Comparer.Tests
{
    public class CompareEqualObjects
    {
        [Fact]
        public void Compare2EqualObjectsShouldNotHaveDifference()
        {
            var simpleObject = new SimpleObject();
            var jobject = JToken.FromObject(simpleObject);
            var jobject2 = JToken.FromObject(simpleObject);

            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

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

            var jobject = JToken.FromObject(array);
            var jobject2 = JToken.FromObject(array);

            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }

        [Fact]
        public void Comparrison2EqualComplextObjectsShouldHaveNoDifference()
        {
            var complexObject = new ComplexObject(true);
            var jobject = JToken.FromObject(complexObject);
            var jobject2 = JToken.FromObject(complexObject);

            var compareResult = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jobject, jobject2);

            compareResult.ComparrisonResult.Should().Be(ComparisonResult.Identical, because: "The JObjects are created from the same CLR object instance");
        }
    }
}