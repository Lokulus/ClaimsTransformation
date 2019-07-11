namespace ClaimsTransformation.Language.DOM
{
    public abstract class IssueExpression : Expression
    {
        public IssueExpression(IssueDuration duration)
        {
            this.Duration = duration;
        }

        public IssueDuration Duration { get; private set; }
    }

    public enum IssueDuration
    {
        None,
        Temporary,
        Permanent
    }
}
