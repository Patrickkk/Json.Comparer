namespace Newtonsoft.Json.Comparer
{
    public class JoinResultWithKey<T, TKey>
    {
        public TKey Key { get; set; }

        public T Value1 { get; set; }

        public T Value2 { get; set; }
    }
}