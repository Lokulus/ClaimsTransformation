using ClaimsTransformation.Language.DOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsTransformation.Language.Parser
{
    public static class ExpressionFactory
    {
        public static LiteralExpression Literal(TokenValue value)
        {
            return new LiteralExpression(value.Value);
        }

        public static PropertyExpression Property(TokenValue value)
        {
            var args = Expressions(value.Children);
            if (args.Length != 2)
            {
                throw new NotImplementedException();
            }
            var source = args[0] as LiteralExpression;
            var name = args[1] as LiteralExpression;
            return new PropertyExpression(source, name);
        }

        public static BinaryExpression Binary(TokenValue value)
        {
            var args = Expressions(value.Children);
            return Binary(args);
        }

        private static BinaryExpression Binary(Expression[] expressions)
        {
            var queue = new Queue<Expression>(expressions);
            var left = default(Expression);
            var @operator = default(LiteralExpression);
            var right = default(Expression);
            while (queue.Count > 0)
            {
                if (left == null)
                {
                    left = queue.Dequeue();
                }
                else if (@operator == null)
                {
                    @operator = queue.Dequeue() as LiteralExpression;
                }
                else if (right == null)
                {
                    right = queue.Dequeue();
                }
                else
                {
                    if (queue.Count > 1)
                    {
                        right = Binary(new[] { right, queue.Dequeue(), queue.Dequeue() });
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            return new BinaryExpression(left, @operator, right);
        }

        public static ConditionExpression Condition(TokenValue value)
        {
            var args = Expressions(value.Children);
            var identifier = args.OfType<LiteralExpression>().FirstOrDefault();
            var expressions = args.OfType<BinaryExpression>();
            return new ConditionExpression(identifier, expressions);
        }

        public static IssueExpression Issue(TokenValue value)
        {
            var args = Expressions(value.Children);
            var issuance = args.OfType<LiteralExpression>().FirstOrDefault();
            var expressions = args.OfType<BinaryExpression>();
            return new IssueExpression(issuance, expressions);
        }

        public static RuleExpression Rule(TokenValue value)
        {
            var args = Expressions(value.Children);
            var conditions = args.OfType<ConditionExpression>();
            var issue = args.OfType<IssueExpression>().FirstOrDefault();
            return new RuleExpression(conditions, issue);
        }

        private static Expression[] Expressions(IEnumerable<TokenValue> values)
        {
            var expressions = new List<Expression>();
            foreach (var value in values)
            {
                if (value.Syntax.Factory != null)
                {
                    expressions.Add(value.Syntax.Factory(value));
                }
                else if (value.Children != null && value.Children.Any())
                {
                    expressions.AddRange(Expressions(value.Children));
                }
            }
            return expressions.ToArray();
        }
    }
}
