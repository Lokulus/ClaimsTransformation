using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class ConditionExpression : Expression
    {
        public const string UNDEFINED = "UNDEFINED";

        public ConditionExpression(string identifier, IEnumerable<BinaryExpression> expressions)
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

        public string Identifier { get; private set; }

        public BinaryExpression[] Expressions { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Identifier))
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
            if (!string.IsNullOrEmpty(this.Identifier))
            {
                builder.Append(this.Identifier);
            }
            else
            {
                builder.Append(UNDEFINED);
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
            if (!string.Equals(this.Identifier, other.Identifier, StringComparison.OrdinalIgnoreCase))
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
