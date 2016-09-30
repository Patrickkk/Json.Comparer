using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Comparer
{
    public class JTokenComparer
    {
        private readonly IArrayKeySelector arrayKeySelector;

        public JTokenComparer(IArrayKeySelector arrayKeySelector)
        {
            this.arrayKeySelector = arrayKeySelector;
        }

        public virtual JTokenComparrisonResult Compare(object object1, object object2)
        {
            var Jobject1 = JToken.FromObject(object1);
            var Jobject2 = JToken.FromObject(object2);
            return CompareTokens("root", Jobject1, Jobject2);
        }

        /// <summary>
        /// Compares the given tokens
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token1"></param>
        /// <param name="token2"></param>
        /// <returns></returns>
        public virtual JTokenComparrisonResult CompareTokens(string key, JToken token1, JToken token2)
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual JTokenComparrisonResult CompareTokens(JoinResultWithKey<JToken, string> values)
        {
            return CompareTokens(values.Key, values.Value1, values.Value2);
        }

        /// <summary>
        /// Compares 2 JValue tokens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token1"></param>
        /// <param name="token2"></param>
        /// <returns></returns>
        public virtual JValueComparrisonResult CompareValue(string key, JValue token1, JValue token2)
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

        /// <summary>
        /// Compares 2 JArray tokens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token1"></param>
        /// <param name="token2"></param>
        /// <returns></returns>
        public virtual JArrayComparrisonResult CompareArrays(string key, JArray token1, JArray token2)
        {
            if (token1 == null) { return new JArrayComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource1 }; }
            if (token2 == null) { return new JArrayComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource2 }; }

            var arrayContentComparrisonResult = OuterJoinStringifyKey(token1.Children(), token2.Children(), arrayKeySelector.SelectArrayKey)
                .Select(CompareTokens);

            return new JArrayComparrisonResult
            {
                Key = key,
                ComparrisonResult = ComparrisonResultFromCollection(arrayContentComparrisonResult),
                ArrayElementComparrisons = arrayContentComparrisonResult
            };
        }

        /// <summary>
        /// Compares 2 JObject tokens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public virtual JTokenComparrisonResult CompareObjects(string key, JObject object1, JObject object2)
        {
            if (object1 == null) { return new JObjectComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource1 }; }
            if (object2 == null) { return new JObjectComparrisonResult { Key = key, ComparrisonResult = ComparisonResult.MissingInSource2 }; }

            var propertyComparrison = OuterJoin(object1.Children<JProperty>(), object2.Children<JProperty>(), (x, y) => x.Name)
                                          .Select(CompareProperty).ToList();
            return new JObjectComparrisonResult
            {
                Key = key,
                PropertyComparrisons = propertyComparrison,
                ComparrisonResult = ComparrisonResultFromCollection(propertyComparrison)
            };
        }

        /// <summary>
        /// Get the comparrisonResult from the collection. returns equal if all elements are present and equal. returns difference if any element is missing or different.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public virtual ComparisonResult ComparrisonResultFromCollection(IEnumerable<JTokenComparrisonResult> collection)
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

        /// <summary>
        /// Compares 2 JProprty tokens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="property1"></param>
        /// <param name="property2"></param>
        /// <returns></returns>
        public virtual JPropertyComparrisonResult CompareProperty(string key, JProperty property1, JProperty property2)
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

        /// <summary>
        /// Joins 2 collections using the keyselector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public virtual IEnumerable<JoinResultWithKey<T, TKey>> OuterJoin<T, TKey>(IEnumerable<T> source1, IEnumerable<T> source2, Func<T, int, TKey> keySelector)
            where T : class
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            var source1WithIndex = source1.Select((value, index) => new { value, key = keySelector(value, index) });
            var source2WithIndex = source2.Select((value, index) => new { value, key = keySelector(value, index) });

            var keys = source1WithIndex.Concat(source2WithIndex).Select(x => x.key).OrderBy(x => x).Distinct();
            var duplicateKeys = source1WithIndex.Concat(source2WithIndex).Select(x => x.key).GroupBy(x => x).Where(x => x.Count() > 2);

            if (duplicateKeys.Any())
            {
                throw new ArgumentOutOfRangeException($"duplicate keys when joining found. The following duplicate keys were found {string.Join(",", duplicateKeys)}. cannot join with duplicate keys.");
            }

            var source1Lookup = source1WithIndex.ToLookup(x => x.key);
            var source2Lookup = source2WithIndex.ToLookup(x => x.key);

            return from key in keys
                   from source1Value in source1Lookup[key].DefaultIfEmpty()
                   from source2Value in source2Lookup[key].DefaultIfEmpty()
                   select new JoinResultWithKey<T, TKey> { Key = key, Value1 = source1Value?.value, Value2 = source2Value?.value };
        }

        /// <summary>
        /// Outer join with a string key resulting from a ToString call on the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public virtual IEnumerable<JoinResultWithKey<T, string>> OuterJoinStringifyKey<T, TKey>(IEnumerable<T> source1, IEnumerable<T> source2, Func<T, int, TKey> keySelector)
    where T : class
        {
            return OuterJoin(source1, source2, keySelector)
                .Select(x => new JoinResultWithKey<T, string> { Key = x.Key.ToString(), Value1 = x.Value1, Value2 = x.Value2 });
        }
    }
}