using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.Parser
{
    public class TokenValue
    {
        public TokenValue(Syntax syntax, Token token, string value) : this(syntax, token, value, Enumerable.Empty<TokenValue>())
        {

        }

        public TokenValue(Syntax syntax, IEnumerable<TokenValue> children) : this(syntax, null, null, children)
        {

        }

        public TokenValue(Syntax syntax, Token token, string value, IEnumerable<TokenValue> children)
        {
            this.Syntax = syntax;
            this.Token = token;
            this.Value = value;
            if (children != null)
            {
                this.Children = children.ToArray();
            }
            else
            {
                this.Children = new TokenValue[] { };
            }
        }

        public Syntax Syntax { get; private set; }

        public Token Token { get; private set; }

        public string Value { get; private set; }

        public TokenValue[] Children { get; private set; }

        public void Add(TokenValue value)
        {
            this.Children = this.Children.Concat(
                new[] { value }
            ).ToArray();
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Syntax != null)
                {
                    hashCode += this.Syntax.GetHashCode();
                }
                if (this.Token != null)
                {
                    hashCode += this.Token.GetHashCode();
                }
                if (!string.IsNullOrEmpty(this.Value))
                {
                    hashCode += this.Value.GetHashCode();
                }
                if (this.Children != null)
                {
                    foreach (var child in this.Children)
                    {
                        hashCode += child.GetHashCode();
                    }
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (this.Token != null)
            {
                builder.Append(this.Token.ToString());
                if (!string.Equals(this.Token.Value, this.Value, StringComparison.OrdinalIgnoreCase))
                {
                    builder.Append(" (");
                    if (!string.IsNullOrEmpty(this.Value))
                    {
                        builder.Append(this.Value);
                    }
                    else
                    {
                        builder.Append("{EMPTY}");
                    }
                    builder.Append(")");
                }
            }
            else if (this.Children != null)
            {
                foreach (var child in this.Children)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(child.ToString());
                }
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TokenValue);
        }

        public virtual bool Equals(TokenValue other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Syntax, other.Syntax))
            {
                return false;
            }
            if (!object.Equals(this.Token, other.Token))
            {
                return false;
            }
            if (!string.Equals(this.Value, other.Value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (!Enumerable.SequenceEqual(this.Children, other.Children))
            {
                return false;
            }
            return true;
        }

        public static bool operator ==(TokenValue a, TokenValue b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }
            if ((object)a == null || (object)b == null)
            {
                return false;
            }
            if (object.ReferenceEquals((object)a, (object)b))
            {
                return true;
            }
            return a.Equals(b);
        }

        public static bool operator !=(TokenValue a, TokenValue b)
        {
            return !(a == b);
        }
    }
}
