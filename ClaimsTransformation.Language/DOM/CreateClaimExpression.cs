using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class CreateClaimExpression : IssueExpression
    {
        public CreateClaimExpression(IssueDuration duration, IEnumerable<BinaryExpression> expressions) : base(duration)
        {
            if (expressions != null)
            {
                this.Expressions = expressions.ToArray();
            }
            else
            {
                this.Expressions = new BinaryExpression[] { };
            }
        }

        public BinaryExpression[] Expressions { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            unchecked
            {
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
            builder.Append(base.ToString());
            builder.Append(Terminals.O_BRACKET);
            if (this.Expressions != null)
            {
                foreach (var expression in this.Expressions)
                {
                    builder.Append(expression.ToString());
                }
            }
            builder.Append(Terminals.C_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CreateClaimExpression);
        }

        public virtual bool Equals(CreateClaimExpression other)
        {
            if (!base.Equals(other))
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
