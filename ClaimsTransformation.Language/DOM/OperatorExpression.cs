using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class OperatorExpression : Expression
    {
        public const string UNKNOWN = "UNKNOWN";

        public static readonly IDictionary<Operator, string> TERMINAL_MAP = new Dictionary<Operator, string>()
        {
            { Operator.EqualTo, Terminals.EQ },
            { Operator.NotEqualTo, Terminals.NEQ },
            { Operator.Match, Terminals.REGEXP_MATCH },
            { Operator.NotMatch, Terminals.REGEXP_NOT_MATCH },
            { Operator.Assign, Terminals.ASSIGN },
            { Operator.And, Terminals.AND }
        };

        public OperatorExpression(Operator @operator)
        {
            this.Operator = @operator;
        }

        public Operator Operator { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                hashCode += this.Operator.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var terminal = default(string);
            if (TERMINAL_MAP.TryGetValue(this.Operator, out terminal))
            {
                builder.Append(terminal);
            }
            else
            {
                builder.Append(UNKNOWN);
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as OperatorExpression);
        }

        public virtual bool Equals(OperatorExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.Operator != other.Operator)
            {
                return false;
            }
            return true;
        }
    }

    public enum Operator
    {
        None,
        EqualTo,
        NotEqualTo,
        Match,
        NotMatch,
        Assign,
        And
    }
}
