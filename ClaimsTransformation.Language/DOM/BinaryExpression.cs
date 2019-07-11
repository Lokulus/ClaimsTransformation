using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class BinaryExpression : Expression
    {
        public BinaryExpression(Expression left, OperatorExpression @operator, Expression right)
        {
            this.Left = left;
            this.Operator = @operator;
            this.Right = right;
        }

        public Expression Left { get; private set; }

        public OperatorExpression Operator { get; private set; }

        public Expression Right { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Left != null)
                {
                    hashCode += this.Left.GetHashCode();
                }
                if (this.Operator != null)
                {
                    hashCode += this.Operator.GetHashCode();
                }
                if (this.Right != null)
                {
                    hashCode += this.Right.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (this.Left != null)
            {
                builder.Append(this.Left.ToString());
            }
            if (this.Operator != null)
            {
                builder.Append(this.Operator.ToString());
            }
            if (this.Right != null)
            {
                builder.Append(this.Right.ToString());
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BinaryExpression);
        }

        public virtual bool Equals(BinaryExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Left, other.Left))
            {
                return false;
            }
            if (!object.Equals(this.Operator, other.Operator))
            {
                return false;
            }
            if (!object.Equals(this.Right, other.Right))
            {
                return false;
            }
            return true;
        }
    }
}
