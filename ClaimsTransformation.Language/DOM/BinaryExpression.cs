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
    }
}
