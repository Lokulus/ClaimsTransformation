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
    }
}
