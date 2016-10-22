using NullGuard;

namespace Json.Comparer
{
    /// <summary>
    /// The reult of a outter join of 2 elements based on a given key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class JoinResultWithKey<T, TKey>
    {
        public TKey Key { get; set; }

        [AllowNull]
        public T Value1 { get; set; }

        [AllowNull]
        public T Value2 { get; set; }
    }
}