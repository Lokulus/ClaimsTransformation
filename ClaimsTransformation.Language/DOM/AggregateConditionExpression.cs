using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class AggregateConditionExpression : ConditionExpression
    {
        protected AggregateConditionExpression(string identifier, string name, IEnumerable<BinaryExpression> expressions) : base(identifier, expressions)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    hashCode += this.Name.GetHashCode();
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
            builder.Append(base.ToString());
            builder.Append(Terminals.C_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AggregateConditionExpression);
        }

        public virtual bool Equals(AggregateConditionExpression other)
        {
            if (!base.Equals(other))
            {
                return false;
            }
            if (!string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
    }
}
