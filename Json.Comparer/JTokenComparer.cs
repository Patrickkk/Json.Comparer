using System;
using System.Collections.Generic;
using System.Linq;
using Json.Comparer.ValueConverters;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace Json.Comparer
{
    public class JTokenComparer
    {
        private readonly IArrayKeySelector arrayKeySelector;
        private readonly IEnumerable<IComparisonFilter> filters;
        private readonly IValueConverter valueConverter;
        private readonly IEnumerable<ComparisonResult> filteredMissingComparrisonResults;

        public JTokenComparer(IArrayKeySelector arrayKeySelector, IEnumerable<IComparisonFilter> filters, IEnumerable<ComparisonResult> filteredMissingComparrisonResults, IValueConverter valueConverter)
        {
            this.filteredMissingComparrisonResults = filteredMissingComparrisonResults;
            this.filters = filters;
            this.arrayKeySelector = arrayKeySelector;
            this.valueConverter = valueConverter;
        }

        public JTokenComparer(IArrayKeySelector arrayKeySelector, IEnumerable<IComparisonFilter> filters, IValueConverter valueConverter) : this(arrayKeySelector, filters, Enumerable.Empty<ComparisonResult>(), valueConverter)
        {
        }

        public JTokenComparer(IArrayKeySelector arrayKeySelector, IEnumerable<IComparisonFilter> filters) : this(arrayKeySelector, filters, new NonConvertingConverter())
        {
        }

        public JTokenComparer(IArrayKeySelector arrayKeySelector, IComparisonFilter filter) : this(arrayKeySelector, new IComparisonFilter[] { filter })
        {
        }

        public JTokenComparer(IArrayKeySelector arrayKeySelector) : this(arrayKeySelector, Enumerable.Empty<IComparisonFilter>())
        {
        }

        public JTokenComparer() : this(new IndexArrayKeySelector(), Enumerable.Empty<IComparisonFilter>())
        {
        }

        public virtual JTokenComparisonResult Compare(object object1, object object2)
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
        public virtual JTokenComparisonResult CompareTokens(string key, [AllowNull]JToken token1, [AllowNull]JToken token2)
        {
            var type = token1 == null
                || token1.Type == JTokenType.Null
                ? token2.Type : token1.Type;

            try
            {
                switch (type)
                {
                    case JTokenType.None:
                        throw new NotImplementedException();
                    case JTokenType.Object:
                        return CompareObjects(key, token1, token2);

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
            catch (Exception e)
            {
                var a = e;
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual JTokenComparisonResult CompareTokens(JoinResultWithKey<JToken, string> values)
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
        public virtual JValueComparrisonResult CompareValue(string key, [AllowNull]JValue token1, [AllowNull]JValue token2)
        {
            if (ShouldBeFiltered(key, token1, token2))
            {
                return new JValueComparrisonResult
                {
                    Key = key,
                    Path = FirstNonNullValueOrDefault("", token1?.Path, token2?.Path),
                    Source1Value = token1.Value?.ToString().EmptyIfNull(),
                    Source2Value = token2.Value?.ToString().EmptyIfNull(),
                };
            }
            if (token1 == null) { return new JValueComparrisonResult { Key = key, Path = token2.Path, ComparisonResult = MissingOrFiltered(ComparisonResult.MissingInSource1), Source1Value = null, Source2Value = valueConverter.Convert(token2.Value?.ToString()) }; }
            if (token2 == null) { return new JValueComparrisonResult { Key = key, Path = token1.Path, ComparisonResult = MissingOrFiltered(ComparisonResult.MissingInSource2), Source1Value = valueConverter.Convert(token1.Value?.ToString()), Source2Value = null }; }

            return new JValueComparrisonResult
            {
                Key = key,
                Path = token1.Path,
                Source1Value = valueConverter.Convert(token1.Value?.ToString()),
                Source2Value = valueConverter.Convert(token2.Value?.ToString()),
                ComparisonResult = valueConverter.Convert(token1.Value?.ToString()) == valueConverter.Convert(token2.Value?.ToString()) ? ComparisonResult.Identical : ComparisonResult.Different
            };
        }

        private ComparisonResult MissingOrFiltered(ComparisonResult result)
        {
            if (filteredMissingComparrisonResults.Contains(result))
            {
                return ComparisonResult.Filtered;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Compares 2 JArray tokens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token1"></param>
        /// <param name="token2"></param>
        /// <returns></returns>
        public virtual JArrayComparrisonResult CompareArrays(string key, [AllowNull]JArray token1, [AllowNull]JArray token2)
        {
            if (MissingOrFiltered(key, token1, token2))
            {
                return MissingOrFilteredResult<JArrayComparrisonResult>(key, token1, token2);
            }

            var arrayContentComparrisonResult = OuterJoinStringifyKey(token1.Children(), token2.Children(), arrayKeySelector.SelectArrayKey)
                .Select(CompareTokens);

            return new JArrayComparrisonResult
            {
                Key = key,
                Path = token1.Path,
                ComparisonResult = ComparisonResultFromCollection(arrayContentComparrisonResult),
                ArrayElementComparrisons = arrayContentComparrisonResult
            };
        }

        private JTokenComparisonResult CompareObjects(string key, JToken token1, JToken token2)
        {
            return CompareObjects(key, token1 as JObject, token2 as JObject);
        }

        /// <summary>
        /// Compares 2 JObject tokens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public virtual JObjectComparisonResult CompareObjects(string key, [AllowNull]JObject object1, [AllowNull]JObject object2)
        {
            if (MissingOrFiltered(key, object1, object2))
            {
                return MissingOrFilteredResult<JObjectComparisonResult>(key, object1, object2);
            }

            var propertyComparison = OuterJoin(object1.Children<JProperty>(), object2.Children<JProperty>(), (x, y) => x.Name)
                                          .Select(CompareProperty).ToList();
            return new JObjectComparisonResult
            {
                Key = key,
                Path = object1.Path,
                PropertyComparisons = propertyComparison,
                ComparisonResult = ComparisonResultFromCollection(propertyComparison)
            };
        }

        /// <summary>
        /// Get the comparrisonResult from the collection. returns equal if all elements are present and equal. returns difference if any element is missing or different.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public virtual ComparisonResult ComparisonResultFromCollection(IEnumerable<JTokenComparisonResult> collection)
        {
            if (collection.Any(x => x.ComparisonResult != ComparisonResult.Identical && x.ComparisonResult != ComparisonResult.Filtered))
            {
                return ComparisonResult.Different;
            }
            else
            {
                return ComparisonResult.Identical;
            }
        }

        private JPropertyComparisonResult CompareProperty(JoinResultWithKey<JProperty, string> joinedProperty)
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
        public virtual JPropertyComparisonResult CompareProperty(string key, [AllowNull]JProperty property1, [AllowNull]JProperty property2)
        {
            if (MissingOrFiltered(key, property1, property2))
            {
                return MissingOrFilteredResult<JPropertyComparisonResult>(key, property1, property2)
                    .With(x => x.Name = FirstNonNullValueOrDefault(property1?.Name, property2?.Name));
            }

            var comparisonResult = CompareTokens(key, property1.Value, property2.Value);

            return new JPropertyComparisonResult
            {
                Key = key,
                Path = property1.Path,
                Name = property1.Name,
                ComparisonResult = comparisonResult.ComparisonResult,
                PropertyValueComparisonResult = comparisonResult
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
                throw new ArgumentOutOfRangeException($"duplicate keys when joining found. The following duplicate keys were found {string.Join(",", duplicateKeys.Select(x => x.Key))}. cannot join with duplicate keys.");
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

        private bool MissingOrFiltered(string key, JToken token1, JToken token2)
        {
            return MissingToken(token1, token2) ||
                ShouldBeFiltered(key, token1, token2);
        }

        private TComparison MissingOrFilteredResult<TComparison>(string key, JToken token1, JToken token2)
    where TComparison : JTokenComparisonResult, new()
        {
            var result = Activator.CreateInstance<TComparison>();
            result.ComparisonResult = MissingOrFilteredComparisonResult(key, token1, token2);
            result.Path = FirstNonNullValue(token1?.Path, token2?.Path);
            result.Key = key;
            return result;
        }

        private ComparisonResult MissingOrFilteredComparisonResult(string key, JToken token1, JToken token2)
        {
            if (ShouldBeFiltered(key, token1, token2)) { return ComparisonResult.Filtered; }
            if (token1 == null) { return MissingOrFiltered(ComparisonResult.MissingInSource1); }
            if (token2 == null) { return MissingOrFiltered(ComparisonResult.MissingInSource2); }
            throw new Exception("This shouldn't happen.");
        }

        private static bool MissingToken(JToken token1, JToken token2)
        {
            return token1 == null
                || token2 == null;
        }

        private bool ShouldBeFiltered(string key, [AllowNull]JToken token1, [AllowNull]JToken token2)
        {
            return filters.Any(filter => filter.ShouldBeFiltered(key, token1, token2));
        }

        private static T FirstNonNullValue<T>(params T[] values)
            where T : class
        {
            return values.First(x => x != null);
        }

        private static T FirstNonNullValueOrDefault<T>(T defaultValue, params T[] values)
            where T : class
        {
            var value = values.FirstOrDefault(x => x != null);
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }
    }
}