using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class UnaryExpression : Expression
    {
        public UnaryExpression(OperatorExpression @operator, Expression expression)
        {
            this.Operator = @operator;
            this.Expression = expression;
        }

        public OperatorExpression Operator { get; private set; }

        public Expression Expression { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Operator != null)
                {
                    hashCode += this.Operator.GetHashCode();
                }
                if (this.Expression != null)
                {
                    hashCode += this.Expression.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (this.Operator != null)
            {
                builder.Append(this.Operator.ToString());
            }
            if (this.Expression != null)
            {
                builder.Append(this.Expression.ToString());
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as UnaryExpression);
        }

        public virtual bool Equals(UnaryExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Operator, other.Operator))
            {
                return false;
            }
            if (!object.Equals(this.Expression, other.Expression))
            {
                return false;
            }
            return true;
        }
    }
}
