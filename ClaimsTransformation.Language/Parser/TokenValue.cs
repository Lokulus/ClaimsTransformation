using System.Collections.Generic;
using System.Linq;

namespace ClaimsTransformation.Language.Parser
{
    public class TokenValue
    {
        public TokenValue(Token token, string value) : this(token, value, Enumerable.Empty<TokenValue>())
        {

        }

        public TokenValue(IEnumerable<TokenValue> children) : this(null, null, children)
        {

        }

        public TokenValue(Token token, string value, IEnumerable<TokenValue> children)
        {
            this.Token = token;
            this.Value = value;
            this.Children = children;
        }

        public Token Token { get; private set; }

        public string Value { get; private set; }

        public IEnumerable<TokenValue> Children { get; private set; }
    }
}
