using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class ConditionExpression : Expression
    {
        public ConditionExpression(LiteralExpression identifier, IEnumerable<BinaryExpression> expressions)
        {
            this.Identifier = identifier;
            if (expressions != null)
            {
                this.Expressions = expressions.ToArray();
            }
            else
            {
                this.Expressions = new BinaryExpression[] { };
            }
        }

        public LiteralExpression Identifier { get; private set; }

        public BinaryExpression[] Expressions { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Identifier != null)
                {
                    hashCode += this.Identifier.GetHashCode();
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
            if (this.Identifier != null)
            {
                builder.Append(this.Identifier.ToString());
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            builder.Append(Terminals.COLON);
            builder.Append(Terminals.O_SQ_BRACKET);
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
            builder.Append(Terminals.C_SQ_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ConditionExpression);
        }

        public virtual bool Equals(ConditionExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Identifier, other.Identifier))
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
