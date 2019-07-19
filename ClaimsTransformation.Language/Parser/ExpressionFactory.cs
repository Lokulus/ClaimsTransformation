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
            var result = default(BinaryExpression);
            var args = new Queue<Expression>(Expressions(value.Children));
            while (args.Count > 0)
            {
                if (result == null)
                {
                    if (args.Count >= 3)
                    {
                        result = new BinaryExpression(
                            args.Dequeue(),
                            args.Dequeue() as LiteralExpression,
                            args.Dequeue()
                        );
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    if (args.Count >= 2)
                    {
                        result = new BinaryExpression(
                            result.Left,
                            result.Operator,
                            new BinaryExpression(
                                result.Right,
                                args.Dequeue() as LiteralExpression,
                                args.Dequeue()
                            )
                        );
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            return result;
        }

        public static CallExpression Call(TokenValue value)
        {
            var args = Expressions(value.Children);
            var name = args.OfType<LiteralExpression>().FirstOrDefault();
            var arguments = args.Except(new[] { name });
            return new CallExpression(name, arguments);
        }

        public static ConditionExpression Condition(TokenValue value)
        {
            var args = Expressions(value.Children);
            var identifier = args.OfType<LiteralExpression>().FirstOrDefault();
            var expressions = args.OfType<BinaryExpression>();
            return new ConditionExpression(identifier, expressions);
        }

        public static AggregateConditionExpression AggregateCondition(TokenValue value)
        {
            var args = Expressions(value.Children);
            var literals = args.OfType<LiteralExpression>().ToArray();
            var expressions = args.OfType<BinaryExpression>();
            switch (literals.Length)
            {
                case 2:
                    return new AggregateConditionExpression(literals[0], literals[1], expressions);
                case 4:
                    return new AggregateConditionExpression(literals[0], literals[1], expressions, literals[2], literals[3]);
            }
            throw new NotImplementedException();
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
