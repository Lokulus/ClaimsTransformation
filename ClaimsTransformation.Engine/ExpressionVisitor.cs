using ClaimsTransformation.Language.DOM;
using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ExpressionVisitor
    {
        public ExpressionVisitor(IClaimsTransformationContext context)
        {
            this.Context = context;
            this.CurrentClaims = new List<Claim>(context.Input);
        }

        public IClaimsTransformationContext Context { get; private set; }

        public IList<Claim> CurrentClaims { get; private set; }

        public Claim CurrentClaim { get; private set; }

        public object Visit(Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Literal:
                    return this.Visit(expression as LiteralExpression);
                case ExpressionType.Propery:
                    return this.Visit(expression as PropertyExpression);
                case ExpressionType.Unary:
                    return this.Visit(expression as UnaryExpression);
                case ExpressionType.Binary:
                    return this.Visit(expression as BinaryExpression);
                case ExpressionType.Call:
                    return this.Visit(expression as CallExpression);
                case ExpressionType.Condition:
                    return this.Visit(expression as ConditionExpression);
                case ExpressionType.Issue:
                    return this.Visit(expression as IssueExpression);
                case ExpressionType.Rule:
                    return this.Visit(expression as RuleExpression);
                default:
                    throw new NotImplementedException();
            }
        }

        public object Visit(LiteralExpression expression)
        {
            return expression.Value;
        }

        public object Visit(PropertyExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(UnaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(CallExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(ConditionExpression expression)
        {
            if (!expression.IsEmpty)
            {
                var identifier = Convert.ToString(this.Visit(expression.Identifier));
                var predicate = this.BuildPredicate(expression.Expressions);
                this.Filter(identifier, predicate);
            }
            return expression;
        }

        public object Visit(IssueExpression expression)
        {
            var issuance = Convert.ToString(this.Visit(expression.Issuance));
            if (string.Equals(issuance, Terminals.ADD, StringComparison.OrdinalIgnoreCase))
            {

            }
            else if (string.Equals(issuance, Terminals.ISSUE, StringComparison.OrdinalIgnoreCase))
            {

            }
            else
            {
                throw new NotImplementedException();
            }
            return expression;
        }

        public object Visit(RuleExpression expression)
        {
            foreach (var condition in expression.Conditions)
            {
                this.Visit(condition);
            }
            this.Visit(expression.Issue);
            return expression;
        }

        protected virtual Func<Claim, bool> BuildPredicate(BinaryExpression[] expressions)
        {
            return claim =>
            {
                foreach (var expression in expressions)
                {
                    if (!this.BuildPredicate(expression)(claim))
                    {
                        return false;
                    }
                }
                return true;
            };
        }

        protected virtual Func<Claim, bool> BuildPredicate(BinaryExpression expression)
        {
            return claim =>
            {
                this.CurrentClaim = claim;
                try
                {
                    return Convert.ToBoolean(this.Visit(expression));
                }
                finally
                {
                    this.CurrentClaim = null;
                }
            };
        }

        protected virtual void Filter(string identifier, Func<Claim, bool> predicate)
        {
            var result = new List<Claim>();
            foreach (var claim in this.CurrentClaims)
            {
                if (!predicate(claim))
                {
                    continue;
                }
                throw new NotImplementedException();
            }
            this.CurrentClaims = result;
        }
    }
}
