﻿using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;

namespace ClaimsTransformation.Engine
{
    public static class ExpressionEvaluator
    {
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
