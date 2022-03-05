using FluentAssertions;
using Json.Comparer.Tests.TestObjects;
using Json.Comparer.TextResultReporter;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Json.Comparer
{
    public class NullComparrisonTests
    {
        [Fact]
        public void CompareTwoPropertiesThatAreJTokenNull()
        {
            var jObject1 = new JObject();
            jObject1.Add("Prop", new JValue((object)null));
            var jObject2 = new JObject();
            jObject2.Add("Prop", new JValue((object)null));

            var result = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jObject1, jObject2);
            result.ComparrisonResult.Should().Be(ComparisonResult.Identical);
            var resultReport = ComparrisonResultTextExporter.Report(result, new ComparisonResult[] { ComparisonResult.Different, ComparisonResult.DifferentTypes, ComparisonResult.Filtered, ComparisonResult.Identical, ComparisonResult.MissingInSource1, ComparisonResult.MissingInSource2 }, new ReporterSettings { Source1Name = "QA", Source2Name = "PROD" });
        }

        [Fact]
        public void CompareTwoPropertiesOneThatIsJtokenNull()
        {
            var jObject1 = new JObject();
            jObject1.Add("Prop", new JValue((object)null));
            var jObject2 = new JObject();
            jObject2.Add("Prop", JObject.FromObject(new ComplexObject(false)));

            var result = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jObject1, jObject2);
            result.ComparrisonResult.Should().Be(ComparisonResult.Different);
            var resultReport = ComparrisonResultTextExporter.Report(result, new ComparisonResult[] { ComparisonResult.Different, ComparisonResult.DifferentTypes, ComparisonResult.Filtered, ComparisonResult.Identical, ComparisonResult.MissingInSource1, ComparisonResult.MissingInSource2 }, new ReporterSettings { Source1Name = "QA", Source2Name = "PROD" });
        }

        [Fact]
        public void CompareTwoPropertiesSecondThatIsJtokenNull()
        {
            var jObject1 = new JObject();
            jObject1.Add("Prop", JObject.FromObject(new ComplexObject(false)));
            var jObject2 = new JObject();
            jObject2.Add("Prop", new JValue((object)null));

            var result = new JTokenComparer(new IndexArrayKeySelector()).CompareTokens("root", jObject1, jObject2);
            result.ComparrisonResult.Should().Be(ComparisonResult.Different);
            var resultReport = ComparrisonResultTextExporter.Report(result, new ComparisonResult[] { ComparisonResult.Different, ComparisonResult.DifferentTypes, ComparisonResult.Filtered, ComparisonResult.Identical, ComparisonResult.MissingInSource1, ComparisonResult.MissingInSource2 }, new ReporterSettings { Source1Name = "QA", Source2Name = "PROD" });
        }
    }
}