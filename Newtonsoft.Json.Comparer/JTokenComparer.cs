using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer
{
    public class JTokenComparer
    {
        readonly IArrayKeySelector arrayKeySelector;

        public JTokenComparer(IArrayKeySelector arrayKeySelector)
        {
            this.arrayKeySelector = arrayKeySelector;
        }
        public JTokenComparrisonResult CompareTokens(string key, JToken token1, JToken token2)
        {
            if (token1 != null && token2 != null)
            {
                if (token1.Type != token2.Type)
                {
                    throw new NotImplementedException("token1.Type != token2.Type");
                }
            }

            switch (token1.Type)
            {
                case JTokenType.None:
                    throw new NotImplementedException();
                case JTokenType.Object:
                    return CompareObjects(key, (JObject)token1, (JObject)token2);

                case JTokenType.Array:
                    return CompareArrays(key, (JArray)token1, (JArray)token2);

                case JTokenType.Property:
                    return CompareProperty(key, (JProperty)token1, (JProperty)token2);

                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.Date:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    return CompareValue(key, (JValue)token1, (JValue)token2);

                default:
                    throw new PlatformNotSupportedException($"Dunno yet {token1.Type}");
            }
        }

        public JTokenComparrisonResult CompareTokens(JoinResultWithKey<JToken, string> values)
        {
            return CompareTokens(values.Key, values.Value1, values.Value2);
        }

        public JTokenComparrisonResult CompareTokens(JoinResultWithKey<JToken, int> values)
        {
            return CompareTokens(values.Key.ToString(), values.Value1, values.Value2);
        }

        private JValueComparrisonResult CompareValue(string key, JValue token1, JValue token2)
        {
            if (token1 == null) { return new JValueComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource1, Source1Value = null, Source2Value = token2.Value?.ToString() }; }
            if (token2 == null) { return new JValueComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource2, Source1Value = token1.Value?.ToString(), Source2Value = null }; }

            return new JValueComparrisonResult
            {
                Key = key,
                Source1Value = token1.Value?.ToString(),
                Source2Value = token2.Value?.ToString(),
                ComparrisonResult = token1.Value?.ToString() == token2.Value?.ToString() ? ComparisonResult.Identical : ComparisonResult.Different
            };
        }

        public JArrayComparrisonResult CompareArrays(string key, JArray token1, JArray token2)
        {
            if (token1 == null) { return new JArrayComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource1 }; }
            if (token2 == null) { return new JArrayComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource2 }; }

            var arrayContentComparrisonResult = OuterJoinStringifyKey(token1.Children(), token2.Children(), arrayKeySelector.SelectArrayKey)
                .Select(CompareTokens);

            return new JArrayComparrisonResult
            {
                Key = key,
                ComparrisonResult = ComparrisonResultFromCollection(arrayContentComparrisonResult),
                ComparrisonResults = arrayContentComparrisonResult
            };
        }

        public JTokenComparrisonResult CompareObjects(string key, JObject object1, JObject object2)
        {
            if (object1 == null) { return new JObjectComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource1 }; }
            if (object2 == null) { return new JObjectComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource2 }; }

            var propertyComparrison = OuterJoin(object1.Children<JProperty>(), object2.Children<JProperty>(), (x, y) => x.Name)
                                          .Select(CompareProperty).ToList();
            return new JObjectComparrisonResult
            {
                Key = key,
                PropertyComparrison = propertyComparrison,
                ComparrisonResult = ComparrisonResultFromCollection(propertyComparrison)
            };
        }

        private ComparisonResult ComparrisonResultFromCollection(IEnumerable<JTokenComparrisonResult> collection)
        {
            if (collection.Any(x => x.ComparrisonResult != ComparisonResult.Identical))
            {
                return ComparisonResult.Different;
            }
            else
            {
                return ComparisonResult.Identical;
            }
        }

        private JPropertyComparrisonResult CompareProperty(JoinResultWithKey<JProperty, string> joinedProperty)
        {
            return CompareProperty(joinedProperty.Key, joinedProperty.Value1, joinedProperty.Value2);
        }

        private JPropertyComparrisonResult CompareProperty(string key, JProperty property1, JProperty property2)
        {
            if (property1 == null) { return new JPropertyComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource1 }; }
            if (property2 == null) { return new JPropertyComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource2 }; }

            var comparrisonResult = CompareTokens(key, property1.Value, property2.Value);

            return new JPropertyComparrisonResult
            {
                Key = key,
                ComparrisonResult = comparrisonResult.ComparrisonResult,
                PropertyValueComparissonResult = comparrisonResult
            };
        }

        public IEnumerable<JoinResultWithKey<T, TKey>> OuterJoin<T, TKey>(IEnumerable<T> source1, IEnumerable<T> source2, Func<T, int, TKey> keySelector)
            where T : class
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            // TODO check duplicates

            var source1WithIndex = source1.Select((value, index) => new { value, key = keySelector(value, index) });
            var source2WithIndex = source2.Select((value, index) => new { value, key = keySelector(value, index) });

            var keys = source1WithIndex.Concat(source2WithIndex).Select(x => x.key).OrderBy(x => x).Distinct();
            var source1Lookup = source1WithIndex.ToLookup(x => x.key);
            var source2Lookup = source2WithIndex.ToLookup(x => x.key);

            return from key in keys
                   from source1Value in source1Lookup[key].DefaultIfEmpty()
                   from source2Value in source2Lookup[key].DefaultIfEmpty()
                   select new JoinResultWithKey<T, TKey> { Key = key, Value1 = source1Value?.value, Value2 = source2Value?.value };
        }

        public IEnumerable<JoinResultWithKey<T, string>> OuterJoinStringifyKey<T, TKey>(IEnumerable<T> source1, IEnumerable<T> source2, Func<T, int, TKey> keySelector)
    where T : class
        {
            return OuterJoin(source1, source2, keySelector)
                .Select(x => new JoinResultWithKey<T, string> { Key = x.Key.ToString(), Value1 = x.Value1, Value2 = x.Value2 });
        }
    }
}