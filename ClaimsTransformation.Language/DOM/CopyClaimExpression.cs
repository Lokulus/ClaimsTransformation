using ClaimsTransformation.Language.Parser;
using System;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class CopyClaimExpression : IssueExpression
    {
        public const string UNDEFINED = "UNDEFINED";

        public CopyClaimExpression(IssueDuration duration, string identifier) : base(duration)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Identifier))
                {
                    hashCode += this.Identifier.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append(Terminals.O_BRACKET);
            builder.Append(Terminals.CLAIM);
            builder.Append(Terminals.ASSIGN);
            if (!string.IsNullOrEmpty(this.Identifier))
            {
                builder.Append(this.Identifier);
            }
            else
            {
                builder.Append(UNDEFINED);
            }
            builder.Append(Terminals.C_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CopyClaimExpression);
        }

        public virtual bool Equals(CopyClaimExpression other)
        {
            if (!base.Equals(other))
            {
                return false;
            }
            if (!string.Equals(this.Identifier, other.Identifier, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
    }
}
