using ClaimsTransformation.Language.Parser;
using System;

namespace ClaimsTransformation.Engine
{
    public static class ExpressionEvaluator
    {
        public static object Evaluate(ExpressionVisitor visitor, object left, object @operator, object right)
        {
            switch (@operator)
            {
                case Terminals.ASSIGN:
                    return EvaluateAssign(visitor, left, right);
                case Terminals.EQ:
                    return EvaluateEquals(visitor, left, right);
            }
            throw new NotImplementedException();
        }

        public static object EvaluateAssign(ExpressionVisitor visitor, object left, object right)
        {
            return visitor.Property = new ClaimProperty(
                Convert.ToString(left),
                right
            );
        }

        public static object EvaluateEquals(ExpressionVisitor visitor, object left, object right)
        {
            return string.Equals(
                Convert.ToString(left),
                Convert.ToString(right),
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}
