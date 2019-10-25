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

        public static IdentifierExpression Identifier(TokenValue value)
        {
            return new IdentifierExpression(value.Value);
        }

        public static ClaimPropertyExpression ClaimProperty(TokenValue value)
        {
            return new ClaimPropertyExpression(value.Value);
        }

        public static ConditionPropertyExpression ConditionProperty(TokenValue value)
        {
            var args = Expressions(value.Children);
            var identifier = Ensure<IdentifierExpression>(args.OfType<IdentifierExpression>().FirstOrDefault());
            var property = Ensure<ClaimPropertyExpression>(args.OfType<ClaimPropertyExpression>().FirstOrDefault());
            Validate(value, args, identifier, property);
            return new ConditionPropertyExpression(
                identifier,
                property
            );
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
                            Ensure<LiteralExpression>(args.Dequeue()),
                            args.Dequeue()
                        );
                    }
                    else
                    {
                        throw new ExpressionFactoryException(string.Format("Failed to create binary expression: Expected 3 expressions but found {0}.", args.Count));
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
                                Ensure<LiteralExpression>(args.Dequeue()),
                                args.Dequeue()
                            )
                        );
                    }
                    else
                    {
                        throw new ExpressionFactoryException(string.Format("Failed to combine binary expression: Expected 2 expressions but found {0}.", args.Count));
                    }
                }
            }
            return result;
        }

        public static CallExpression Call(TokenValue value)
        {
            var args = Expressions(value.Children);
            var name = Ensure<LiteralExpression>(args.OfType<LiteralExpression>().FirstOrDefault());
            var arguments = args.Except(new[] { name });
            return new CallExpression(name, arguments);
        }

        public static ConditionExpression Condition(TokenValue value)
        {
            var args = Expressions(value.Children);
            var identifier = args.OfType<IdentifierExpression>().FirstOrDefault();
            var expressions = args.OfType<BinaryExpression>();
            Validate(value, args, new Expression[] { identifier }.Concat(expressions));
            return new ConditionExpression(identifier, expressions);
        }

        public static AggregateConditionExpression AggregateCondition(TokenValue value)
        {
            var args = Expressions(value.Children);
            var identifier = args.OfType<IdentifierExpression>().FirstOrDefault();
            var literals = args.OfType<LiteralExpression>().ToArray();
            var expressions = args.OfType<BinaryExpression>();
            Validate(value, args, new Expression[] { identifier }.Concat(literals).Concat(expressions));
            switch (literals.Length)
            {
                case 1:
                    return new AggregateConditionExpression(identifier, literals[0], expressions);
                case 3:
                    return new AggregateConditionExpression(identifier, literals[0], expressions, literals[1], literals[2]);
            }
            throw new ExpressionFactoryException(string.Format("Failed to create aggregate condition expression: Expected 1 or 3 literals but found {0}.", literals.Length));
        }

        public static IssueExpression Issue(TokenValue value)
        {
            var args = Expressions(value.Children);
            var issuance = Ensure<LiteralExpression>(args.OfType<LiteralExpression>().FirstOrDefault());
            var expressions = args.OfType<BinaryExpression>();
            Validate(value, args, new Expression[] { issuance }.Concat(expressions));
            return new IssueExpression(issuance, expressions);
        }

        public static RuleExpression Rule(TokenValue value)
        {
            var args = Expressions(value.Children);
            var conditions = args.OfType<ConditionExpression>();
            var issue = Ensure<IssueExpression>(args.OfType<IssueExpression>().FirstOrDefault());
            Validate(value, args, new Expression[] { issue }.Concat(conditions));
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

        private static T Ensure<T>(Expression expression) where T : Expression
        {
            var result = expression as T;
            if (result == null)
            {
                throw new ExpressionFactoryException(
                    string.Format(
                        "Expected expression of type \"{0}\" but found \"{1}\": {2}",
                        typeof(T).Name,
                        expression != null ? expression.GetType().Name : "None",
                        expression != null ? expression.ToString() : "None"
                    )
                );
            }
            return result;
        }

        private static void Validate(TokenValue value, IEnumerable<Expression> expected, params Expression[] actual)
        {
            Validate(value, expected, actual.AsEnumerable());
        }

        private static void Validate(TokenValue value, IEnumerable<Expression> expected, IEnumerable<Expression> actual)
        {
            foreach (var expression in expected)
            {
                if (!actual.Contains(expression))
                {
                    throw new ExpressionFactoryException(
                        string.Format(
                            "Expression of type \"{0}\" was not handled: {1}",
                            expression.GetType().Name,
                            expression.ToString()
                        )
                    );
                }
            }
        }
    }
}
