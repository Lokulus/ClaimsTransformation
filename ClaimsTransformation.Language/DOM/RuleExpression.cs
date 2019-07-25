using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class RuleExpression : Expression
    {
        public RuleExpression(IEnumerable<ConditionExpression> conditions, IssueExpression issue)
        {
            if (conditions != null)
            {
                this.Conditions = conditions.ToArray();
            }
            else
            {
                this.Conditions = new ConditionExpression[] { };
            }
            this.Issue = issue;
        }

        public ConditionExpression[] Conditions { get; private set; }

        public IssueExpression Issue { get; private set; }

        public override ExpressionType Type
        {
            get
            {
                return ExpressionType.Rule;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            unchecked
            {
                if (this.Conditions != null)
                {
                    foreach (var condition in this.Conditions)
                    {
                        hashCode += condition.GetHashCode();
                    }
                }
                if (this.Issue != null)
                {
                    hashCode += this.Issue.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (this.Conditions != null)
            {
                foreach (var condition in this.Conditions)
                {
                    builder.Append(condition.ToString());
                }
            }
            builder.Append(Terminals.IMPLY);
            if (this.Issue != null)
            {
                builder.Append(this.Issue.ToString());
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as RuleExpression);
        }

        public virtual bool Equals(RuleExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!Enumerable.SequenceEqual(this.Conditions, other.Conditions))
            {
                return false;
            }
            if (!object.Equals(this.Issue, other.Issue))
            {
                return false;
            }
            return true;
        }
    }
}
