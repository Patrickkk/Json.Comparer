using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Comparer
{
    public class TokencomparrisonInput<TJtokenType> : TokencomparrisonInputBase
        where TJtokenType : JToken
    {
        public override string Key { get; set; }

        new public TJtokenType Token1 { get; set; }

        new public TJtokenType Token2 { get; set; }

        public static TokencomparrisonInput<JToken> FromJoinwithKey<TKey>(JoinResultWithKey<TJtokenType, TKey> value)
        {
            return new TokencomparrisonInput<JToken>
            {
                Key = value.Key?.ToString(),
                Token1 = value.Value1,
                Token2 = value.Value2
            };
        }

        public static TokencomparrisonInput<TJtokenTypeCreate> Create<TJtokenTypeCreate>(string key, TJtokenTypeCreate value1, TJtokenTypeCreate value2)
            where TJtokenTypeCreate : JToken
        {
            return new TokencomparrisonInput<TJtokenTypeCreate> { Key = key, Token1 = value1, Token2 = value2 };
        }
    }

    public class TokencomparrisonInputBase
    {
        public virtual string Key { get; set; }

        public virtual JToken Token1 { get; set; }

        public virtual JToken Token2 { get; set; }
    }
}