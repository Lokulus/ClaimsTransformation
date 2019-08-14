using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ClaimsTransformation.Engine
{
    public static class ExpressionEvaluator
    {
        private static readonly IDictionary<string, Func<ExpressionVisitor, IEnumerable<object>, object>> Functions = new Dictionary<string, Func<ExpressionVisitor, IEnumerable<object>, object>>(StringComparer.OrdinalIgnoreCase)
        {
            { Terminals.REGEX_REPLACE, EvaluateRegExReplace }
        };

        public static object Evaluate(ExpressionVisitor visitor, object name, IEnumerable<object> arguments)
        {
            var function = default(Func<ExpressionVisitor, IEnumerable<object>, object>);
            if (!Functions.TryGetValue(Convert.ToString(name), out function))
            {
                throw new NotImplementedException();
            }
            return function(visitor, arguments);
        }

        private static object EvaluateRegExReplace(ExpressionVisitor visitor, IEnumerable<object> arguments)
        {
            var result = new List<object>();
            var args = arguments.ToArray();
            if (args.Length != 3)
            {
                throw new NotImplementedException();
            }
            var inputSequence = default(IEnumerable<object>);
            if (!IsSequence(args[0], out inputSequence))
            {
                inputSequence = new[] { args[0] };
            }
            var pattern = Convert.ToString(args[1]);
            var replacement = Convert.ToString(args[2]);
            foreach (var input in inputSequence)
            {
                result.Add(Regex.Replace(Convert.ToString(input), pattern, replacement));
            }
            switch (result.Count)
            {
                case 0:
                    return null;
                case 1:
                    return result[0];
                default:
                    return result;
            }
        }

        public static object Evaluate(ExpressionVisitor visitor, object left, object @operator, object right)
        {
            var leftSequence = default(IEnumerable<object>);
            var rightSequence = default(IEnumerable<object>);
            if (!IsSequence(left, out leftSequence))
            {
                leftSequence = new[] { left };
            }
            if (!IsSequence(right, out rightSequence))
            {
                rightSequence = new[] { right };
            }
            return Evaluate(visitor, leftSequence, @operator, rightSequence);
        }

        private static object Evaluate(ExpressionVisitor visitor, IEnumerable<object> leftSequence, object @operator, IEnumerable<object> rightSequence)
        {
            var result = new List<object>();
            foreach (var left in leftSequence)
            {
                foreach (var right in rightSequence)
                {
                    switch (@operator)
                    {
                        case Terminals.ASSIGN:
                            result.Add(EvaluateAssign(visitor, left, right));
                            break;
                        case Terminals.EQ:
                            result.Add(EvaluateEquals(visitor, left, right));
                            break;
                        case Terminals.NEQ:
                            result.Add(EvaluateNotEquals(visitor, left, right));
                            break;
                        case Terminals.LESS:
                            result.Add(EvaluateLess(visitor, left, right));
                            break;
                        case Terminals.LESS_EQUAL:
                            result.Add(EvaluateLessOrEqual(visitor, left, right));
                            break;
                        case Terminals.GREATER:
                            result.Add(EvaluateGreater(visitor, left, right));
                            break;
                        case Terminals.GREATER_EQUAL:
                            result.Add(EvaluateGreaterOrEqual(visitor, left, right));
                            break;
                        case Terminals.REGEXP_MATCH:
                            result.Add(EvaluateMatch(visitor, left, right));
                            break;
                        case Terminals.REGEXP_NOT_MATCH:
                            result.Add(EvaluateNotMatch(visitor, left, right));
                            break;
                        case Terminals.CONCAT:
                            result.Add(EvaluateConcat(visitor, left, right));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            switch (result.Count)
            {
                case 0:
                    return null;
                case 1:
                    return result[0];
                default:
                    return result;
            }
        }

        private static object EvaluateAssign(ExpressionVisitor visitor, object left, object right)
        {
            var property = new ClaimProperty(
                Convert.ToString(left),
                right
            );
            return property;
        }

        private static object EvaluateEquals(ExpressionVisitor visitor, object left, object right)
        {
            return string.Equals(
                Convert.ToString(left),
                Convert.ToString(right),
                StringComparison.OrdinalIgnoreCase
            );
        }

        private static object EvaluateNotEquals(ExpressionVisitor visitor, object left, object right)
        {
            return !string.Equals(
                Convert.ToString(left),
                Convert.ToString(right),
                StringComparison.OrdinalIgnoreCase
            );
        }

        private static object EvaluateLess(ExpressionVisitor visitor, object left, object right)
        {
            return Convert.ToInt32(left) < Convert.ToInt32(right);
        }

        private static object EvaluateLessOrEqual(ExpressionVisitor visitor, object left, object right)
        {
            return Convert.ToInt32(left) <= Convert.ToInt32(right);
        }

        private static object EvaluateGreater(ExpressionVisitor visitor, object left, object right)
        {
            return Convert.ToInt32(left) > Convert.ToInt32(right);
        }

        private static object EvaluateGreaterOrEqual(ExpressionVisitor visitor, object left, object right)
        {
            return Convert.ToInt32(left) >= Convert.ToInt32(right);
        }

        private static object EvaluateMatch(ExpressionVisitor visitor, object left, object right)
        {
            return Regex.Match(
                Convert.ToString(left),
                Convert.ToString(right),
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            ).Success;
        }

        private static object EvaluateNotMatch(ExpressionVisitor visitor, object left, object right)
        {
            return !Regex.Match(
                Convert.ToString(left),
                Convert.ToString(right),
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            ).Success;
        }

        private static object EvaluateConcat(ExpressionVisitor visitor, object left, object right)
        {
            return string.Concat(
                Convert.ToString(left),
                Convert.ToString(right)
            );
        }

        private static bool IsSequence(object value, out IEnumerable<object> values)
        {
            values = value as IEnumerable<object>;
            return values != null;
        }
    }
}
