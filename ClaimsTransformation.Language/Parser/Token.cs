using System;
using System.Text;

namespace ClaimsTransformation.Language.Parser
{
    public class Token
    {
        public Token(string value) : this(value, TokenChannel.Discard)
        {

        }

        public Token(string value, TokenChannel channel) : this(value, channel, TokenFlags.Literal)
        {

        }

        public Token(string value, TokenChannel channel, TokenFlags flags)
        {
            this.Value = value;
            this.Channel = channel;
            this.Flags = flags;
        }

        public string Value { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.Value);
            }
        }

        public TokenChannel Channel { get; private set; }

        public TokenFlags Flags { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Value))
                {
                    hashCode += this.Value.GetHashCode();
                }
                hashCode += this.Flags.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Value))
            {
                builder.Append(this.Value);
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            if (this.Flags != TokenFlags.None)
            {
                builder.Append(" (");
                builder.Append(Enum.GetName(typeof(TokenFlags), this.Flags));
                builder.Append(")");
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Token);
        }

        public virtual bool Equals(Token other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!string.Equals(this.Value, other.Value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (this.Flags != other.Flags)
            {
                return false;
            }
            return true;
        }

        public static bool operator ==(Token a, Token b)
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

        public static bool operator !=(Token a, Token b)
        {
            return !(a == b);
        }
    }

    public enum TokenChannel
    {
        None,
        Normal,
        Discard
    }

    [Flags]
    public enum TokenFlags
    {
        None = 0,
        Literal = 1,
        Identifier = 2,
        Boolean = 4,
        Number = 8,
        String = 16
    }
}
