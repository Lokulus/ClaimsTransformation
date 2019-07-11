namespace ClaimsTransformation.Language.DOM
{
    public class OperatorExpression : Expression
    {
        public OperatorExpression(Operator @operator)
        {
            this.Operator = @operator;
        }

        public Operator Operator { get; private set; }
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
