using ClaimsTransformation.Language.DOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsTransformation.Language.Parser
{
    public static class ExpressionFactory
    {
        private static IEnumerable<BinaryExpression> Binaries(TokenValue value)
        {
            throw new NotImplementedException();
        }

        public static ConditionExpression Condition(TokenValue value)
        {
            var identifier = value.Children[0].Value;
            var expressions = default(IEnumerable<BinaryExpression>);
            if (value.Children.Length > 1)
            {
                expressions = Binaries(value.Children[1]);
            }
            else
            {
                expressions = Enumerable.Empty<BinaryExpression>();
            }
            return new ConditionExpression(identifier, expressions);
        }

        public static IEnumerable<ConditionExpression> Conditions(TokenValue value)
        {
            foreach (var child in value.Children)
            {
                yield return Condition(child);
            }
        }

        public static IssueDuration Duration(TokenValue value)
        {
            if (string.Equals(value.Value, Terminals.ISSUE))
            {
                return IssueDuration.Permanent;
            }
            else if (string.Equals(value.Value, Terminals.ADD))
            {
                return IssueDuration.Temporary;
            }
            throw new NotImplementedException();
        }

        public static CreateClaimExpression Create(IssueDuration duration, TokenValue value)
        {
            var expressions = Binaries(value.Children[0]);
            return new CreateClaimExpression(duration, expressions);
        }

        public static CopyClaimExpression Copy(IssueDuration duration, TokenValue value)
        {
            var identifier = value.Children[0].Value;
            return new CopyClaimExpression(duration, identifier);
        }

        public static IssueExpression Issue(TokenValue value)
        {
            var duration = Duration(value.Children[0]);
            if (value.Children[1].Syntax == ClaimsTransformationSyntax.Create)
            {
                return Create(duration, value.Children[1]);
            }
            else if (value.Children[1].Syntax == ClaimsTransformationSyntax.Copy)
            {
                return Copy(duration, value.Children[1]);
            }
            throw new NotImplementedException();
        }

        public static RuleExpression Rule(TokenValue value)
        {
            var conditions = Conditions(value.Children[0]);
            var issue = Issue(value.Children[1]);
            return new RuleExpression(conditions, issue);
        }
    }
}
