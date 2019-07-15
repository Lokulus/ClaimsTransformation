using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class CallExpression : Expression
    {
        public CallExpression(string name, IEnumerable<Expression> arguments)
        {
            this.Name = name;
            this.Arguments = arguments;
        }

        public string Name { get; private set; }

        public IEnumerable<Expression> Arguments { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    hashCode += this.Name.GetHashCode();
                }
                if (this.Arguments != null)
                {
                    foreach (var argument in this.Arguments)
                    {
                        hashCode += argument.GetHashCode();
                    }
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Name))
            {
                builder.Append(this.Name);
            }
            builder.Append(Terminals.O_BRACKET);
            if (this.Arguments != null)
            {
                var first = true;
                foreach (var argument in this.Arguments)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(Terminals.COMMA);
                    }
                    builder.Append(argument.ToString());
                }
            }
            builder.Append(Terminals.C_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CallExpression);
        }

        public virtual bool Equals(CallExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (!Enumerable.SequenceEqual(this.Arguments, other.Arguments))
            {
                return false;
            }
            return true;
        }
    }
}
