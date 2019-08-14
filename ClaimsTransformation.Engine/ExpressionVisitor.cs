using ClaimsTransformation.Language.DOM;
using ClaimsTransformation.Language.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ExpressionVisitor
    {
        public ExpressionVisitor(IClaimsTransformationContext context)
        {
            this.Context = context;
        }

        internal ConditionStates ConditionStates { get; private set; }

        public IClaimsTransformationContext Context { get; private set; }

        public Claim Claim { get; private set; }

        public object Visit(Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Literal:
                    return this.Visit(expression as LiteralExpression);
                case ExpressionType.ClaimPropery:
                    return this.Visit(expression as ClaimPropertyExpression);
                case ExpressionType.ConditionProperty:
                    return this.Visit(expression as ConditionPropertyExpression);
                case ExpressionType.Unary:
                    return this.Visit(expression as UnaryExpression);
                case ExpressionType.Binary:
                    return this.Visit(expression as BinaryExpression);
                case ExpressionType.Call:
                    return this.Visit(expression as CallExpression);
                case ExpressionType.Condition:
                    return this.Visit(expression as ConditionExpression);
                case ExpressionType.AggregateCondition:
                    return this.Visit(expression as AggregateConditionExpression);
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
            if (expression == null)
            {
                return null;
            }
            return expression.Value;
        }

        public object Visit(ClaimPropertyExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (this.Claim == null)
            {
                return expression.Name;
            }
            else if (string.Equals(expression.Name, ClaimProperty.CLAIM, StringComparison.OrdinalIgnoreCase))
            {
                return this.Claim;
            }
            else if (string.Equals(expression.Name, ClaimProperty.TYPE, StringComparison.OrdinalIgnoreCase))
            {
                return this.Claim.Type;
            }
            else if (string.Equals(expression.Name, ClaimProperty.VALUE, StringComparison.OrdinalIgnoreCase))
            {
                return this.Claim.Value;
            }
            else if (string.Equals(expression.Name, ClaimProperty.VALUE_TYPE, StringComparison.OrdinalIgnoreCase))
            {
                return this.Claim.ValueType;
            }
            else if (string.Equals(expression.Name, ClaimProperty.ISSUER, StringComparison.OrdinalIgnoreCase))
            {
                return this.Claim.Issuer;
            }
            else if (string.Equals(expression.Name, ClaimProperty.ORIGINAL_ISSUER, StringComparison.OrdinalIgnoreCase))
            {
                return this.Claim.OriginalIssuer;
            }
            throw new NotImplementedException();
        }

        public object Visit(ConditionPropertyExpression expression)
        {
            var result = new List<object>();
            var source = Convert.ToString(this.Visit(expression.Source));
            foreach (var claim in this.ConditionStates[source].Claims)
            {
                try
                {
                    this.Claim = claim;
                    result.Add(this.Visit(expression.Property));
                }
                finally
                {
                    this.Claim = null;
                }
            }
            return result;
        }

        public object Visit(UnaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(BinaryExpression expression)
        {
            var left = this.Visit(expression.Left);
            var @operator = this.Visit(expression.Operator);
            var right = this.Visit(expression.Right);
            return ExpressionEvaluator.Evaluate(this, left, @operator, right);
        }

        public object Visit(CallExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(ConditionExpression expression)
        {
            var claims = new List<Claim>();
            var identifier = Convert.ToString(this.Visit(expression.Identifier));
            if (!expression.IsEmpty)
            {
                var predicate = this.BuildPredicate(expression.Expressions);
                foreach (var claim in this.Context.Input)
                {
                    if (!predicate(claim))
                    {
                        continue;
                    }
                    claims.Add(claim);
                }
                if (claims.Count > 0)
                {
                    this.ConditionStates[identifier].Claims = claims;
                    this.ConditionStates[identifier].IsMatch = true;
                }
                else
                {
                    this.ConditionStates[identifier].Claims = Enumerable.Empty<Claim>();
                    this.ConditionStates[identifier].IsMatch = false;
                }
            }
            else
            {
                this.ConditionStates[identifier].Claims = this.Context.Input;
                this.ConditionStates[identifier].IsMatch = true;
            }
            return expression;
        }

        public object Visit(AggregateConditionExpression expression)
        {
            this.Visit((ConditionExpression)expression);
            var name = Convert.ToString(this.Visit(expression.Name));
            var identifier = Convert.ToString(this.Visit(expression.Identifier));
            if (string.Equals(name, Terminals.EXISTS, StringComparison.OrdinalIgnoreCase))
            {
                if (this.ConditionStates[identifier].Claims.Any())
                {
                    this.ConditionStates[identifier].Claims = this.ConditionStates[identifier].Claims
                        .Take(1)
                        .ToArray();
                    this.ConditionStates[identifier].IsMatch = true;
                }
                else
                {
                    this.ConditionStates[identifier].Claims = Enumerable.Empty<Claim>();
                    this.ConditionStates[identifier].IsMatch = false;
                }
            }
            else if (string.Equals(name, Terminals.NOT_EXISTS, StringComparison.OrdinalIgnoreCase))
            {
                if (this.ConditionStates[identifier].Claims.Any())
                {
                    this.ConditionStates[identifier].Claims = Enumerable.Empty<Claim>();
                    this.ConditionStates[identifier].IsMatch = false;
                }
                else
                {
                    this.ConditionStates[identifier].Claims = Enumerable.Empty<Claim>();
                    this.ConditionStates[identifier].IsMatch = true;
                }
            }
            else if (string.Equals(name, Terminals.COUNT, StringComparison.OrdinalIgnoreCase))
            {
                var @operator = Convert.ToString(this.Visit(expression.Operator));
                var value = Convert.ToString(this.Visit(expression.Value));
                var result = Convert.ToBoolean(
                    ExpressionEvaluator.Evaluate(
                        this,
                        this.ConditionStates[identifier].Claims.Count(),
                        @operator,
                        value
                    )
                );
                if (result)
                {
                    this.ConditionStates[identifier].IsMatch = true;
                }
                else
                {
                    this.ConditionStates[identifier].IsMatch = false;
                }
            }
            return expression;
        }

        public object Visit(IssueExpression expression)
        {
            var issuance = Convert.ToString(this.Visit(expression.Issuance));
            if (this.ConditionStates.IsMatch)
            {
                var claims = new List<Claim>();
                if (this.IsStaticSelector(expression.Expressions))
                {
                    var selector = this.BuildStaticSelector(expression.Expressions);
                    claims.Add(selector());
                }
                else
                {
                    var selector = this.BuildDynamicSelector(expression.Expressions);
                    claims.AddRange(selector());
                }
                if (this.Context.Output == null)
                {
                    this.Context.Output = claims.ToArray();
                }
                else
                {
                    this.Context.Output = this.Context.Output.Concat(claims).ToArray();
                }
            }
            return expression;
        }

        public object Visit(RuleExpression expression)
        {
            try
            {
                this.ConditionStates = new ConditionStates();
                foreach (var condition in expression.Conditions)
                {
                    this.Visit((Expression)condition);
                }
                this.Visit(expression.Issue);
                return expression;
            }
            finally
            {
                this.ConditionStates = null;
            }
        }

        protected virtual Func<Claim, bool> BuildPredicate(BinaryExpression[] expressions)
        {
            return claim =>
            {
                try
                {
                    this.Claim = claim;
                    foreach (var expression in expressions)
                    {
                        if (!Convert.ToBoolean(this.Visit(expression)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                finally
                {
                    this.Claim = null;
                }
            };
        }

        protected virtual bool IsStaticSelector(BinaryExpression[] expressions)
        {
            return expressions.All(expression => expression.IsStatic);
        }

        protected virtual Func<Claim> BuildStaticSelector(BinaryExpression[] expressions)
        {
            return () =>
            {
                var properties = new List<ClaimProperty>();
                foreach (var expression in expressions)
                {
                    var property = this.Visit(expression) as ClaimProperty;
                    if (property == null)
                    {
                        throw new NotImplementedException();
                    }
                    properties.Add(property);
                }
                return ClaimFactory.Create(properties);
            };
        }

        protected virtual Func<IEnumerable<Claim>> BuildDynamicSelector(BinaryExpression[] expressions)
        {
            return () =>
            {
                var properties = new List<ClaimProperty>();
                foreach (var expression in expressions)
                {
                    var result = this.Visit(expression);
                    if (result is ClaimProperty)
                    {
                        properties.Add(result as ClaimProperty);
                    }
                    else if (result is IEnumerable)
                    {
                        properties.AddRange((result as IEnumerable).OfType<ClaimProperty>());
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                return ClaimProperty.Productize(properties).Select(group => ClaimFactory.Create(group));
            };
        }
    }
}
