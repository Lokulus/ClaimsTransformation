using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class IssueExpression : Expression
    {
        public IssueExpression(LiteralExpression issuance, IEnumerable<BinaryExpression> expressions)
        {
            this.Issuance = issuance;
            if (expressions != null)
            {
                this.Expressions = expressions.ToArray();
            }
            else
            {
                this.Expressions = new BinaryExpression[] { };
            }
        }

        public LiteralExpression Issuance { get; private set; }

        public BinaryExpression[] Expressions { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Issuance != null)
                {
                    hashCode += this.Issuance.GetHashCode();
                }
                if (this.Expressions != null)
                {
                    foreach (var expression in this.Expressions)
                    {
                        hashCode += expression.GetHashCode();
                    }
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (this.Issuance != null)
            {
                builder.Append(this.Issuance.ToString());
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            builder.Append(Terminals.O_BRACKET);
            if (this.Expressions != null)
            {
                var first = true;
                foreach (var expression in this.Expressions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(Terminals.COMMA);
                    }
                    builder.Append(expression.ToString());
                }
            }
            builder.Append(Terminals.C_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IssueExpression);
        }

        public virtual bool Equals(IssueExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Issuance, other.Issuance))
            {
                return false;
            }
            if (!Enumerable.SequenceEqual(this.Expressions, other.Expressions))
            {
                return false;
            }
            return true;
        }
    }
}
