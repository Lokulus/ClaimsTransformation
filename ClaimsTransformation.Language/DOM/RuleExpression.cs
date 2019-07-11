using System.Collections.Generic;

namespace ClaimsTransformation.Language.DOM
{
    public class RuleExpression : Expression
    {
        public RuleExpression(IEnumerable<ConditionExpression> conditions, IssueExpression issue)
        {
            this.Conditions = conditions;
            this.Issue = issue;
        }

        public IEnumerable<ConditionExpression> Conditions { get; private set; }

        public IssueExpression Issue { get; private set; }
    }
}
