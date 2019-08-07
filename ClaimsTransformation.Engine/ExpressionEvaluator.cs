using ClaimsTransformation.Language.Parser;
using System;

namespace ClaimsTransformation.Engine
{
    public static class ExpressionEvaluator
    {
        public static object Evaluate(object left, object @operator, object right)
        {
            switch (@operator)
            {
                case Terminals.EQ:
                    return EvaluateEquals(left, right);
            }
            throw new NotImplementedException();
        }

        public static bool EvaluateEquals(object left, object right)
        {
            return string.Equals(
                Convert.ToString(left),
                Convert.ToString(right),
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}
