using ClaimsTransformation.Language.Parser;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public abstract class IssueExpression : Expression
    {
        public IssueExpression(IssueDuration duration)
        {
            this.Duration = duration;
        }

        public IssueDuration Duration { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                hashCode += this.Duration.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            switch (this.Duration)
            {
                case IssueDuration.Temporary:
                    builder.Append(Terminals.ADD);
                    break;
                case IssueDuration.Permanent:
                    builder.Append(Terminals.ISSUE);
                    break;
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IssueExpression);
        }

        public virtual bool Equals(IssueExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.Duration != other.Duration)
            {
                return false;
            }
            return true;
        }
    }

    public enum IssueDuration
    {
        None,
        Temporary,
        Permanent
    }
}
