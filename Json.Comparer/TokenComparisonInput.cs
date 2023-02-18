using Newtonsoft.Json.Linq;

namespace Json.Comparer
{
    public class TokenComparisonInput<TJtokenType> : TokencomparisonInputBase
        where TJtokenType : JToken
    {
        public override string Key { get; set; }

        new public TJtokenType Token1 { get; set; }

        new public TJtokenType Token2 { get; set; }

        public static TokenComparisonInput<JToken> FromJoinwithKey<TKey>(JoinResultWithKey<TJtokenType, TKey> value)
        {
            return new TokenComparisonInput<JToken>
            {
                Key = value.Key?.ToString(),
                Token1 = value.Value1,
                Token2 = value.Value2
            };
        }

        public static TokenComparisonInput<TJtokenTypeCreate> Create<TJtokenTypeCreate>(string key, TJtokenTypeCreate value1, TJtokenTypeCreate value2)
            where TJtokenTypeCreate : JToken
        {
            return new TokenComparisonInput<TJtokenTypeCreate> { Key = key, Token1 = value1, Token2 = value2 };
        }
    }

    public class TokencomparisonInputBase
    {
        public virtual string Key { get; set; }

        public virtual JToken Token1 { get; set; }

        public virtual JToken Token2 { get; set; }
    }
}