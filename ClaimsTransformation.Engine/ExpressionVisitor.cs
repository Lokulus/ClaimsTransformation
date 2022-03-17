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

        public IEnumerable<object> Visit(IEnumerable<Expression> expressions)
        {
            return expressions.Select(expression => this.Visit(expression)).ToArray();
        }

        public object Visit(Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Literal:
                    return this.Visit(expression as LiteralExpression);
                case ExpressionType.Identifier:
                    return this.Visit(expression as IdentifierExpression);
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

        public object Visit(IdentifierExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            var identifier = expression.Value;
            var claims = default(IEnumerable<Claim>);
            if (!this.ConditionStates.TryGetClaims(expression.Value, out claims))
            {
                throw new ClaimsTransformationException(string.Format("Could not resolve claims with identifier \"{0}\".", identifier));
            }
            return this.Context.ClaimFactory.Create(claims);
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
            var identifier = expression.Identifier.Value;
            var claims = default(IEnumerable<Claim>);
            if (!this.ConditionStates.TryGetClaims(identifier, out claims))
            {
                throw new ClaimsTransformationException(string.Format("Could not resolve claims with identifier \"{0}\".", identifier));
            }
            foreach (var claim in claims)
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
            var name = this.Visit(expression.Name);
            var arguments = this.Visit(expression.Arguments);
            return ExpressionEvaluator.Evaluate(this, name, arguments);
        }

        public object Visit(ConditionExpression expression)
        {
            var claims = new List<Claim>();
            var identifier = default(string);
            if (expression.Identifier != null)
            {
                identifier = expression.Identifier.Value;
            }
            else
            {
                identifier = string.Empty;
            }
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
            var name = expression.Name.Value;
            var identifier = default(string);
            if (expression.Identifier != null)
            {
                identifier = expression.Identifier.Value;
            }
            else
            {
                identifier = string.Empty;
            }
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
                    if (this.Context.Flags.HasFlag(ClaimsTransformationFlags.UnconditionalExistsIsAlwaysTrue))
                    {
                        this.ConditionStates[identifier].IsMatch = true;
                    }
                    else
                    {
                        this.ConditionStates[identifier].IsMatch = false;
                    }
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
                var @operator = expression.Operator.Value;
                var value = expression.Value.Value;
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
            var issuance = expression.Issuance.Value;
            if (this.ConditionStates.IsMatch)
            {
                var selector = this.BuildSelector(expression.Expressions);
                var claims = selector();
                if (string.Equals(issuance, Terminals.ADD, StringComparison.OrdinalIgnoreCase))
                {
                    this.Context.Add(ClaimStore.Transient, claims);
                }
                else if (string.Equals(issuance, Terminals.ISSUE, StringComparison.OrdinalIgnoreCase))
                {
                    this.Context.Add(ClaimStore.Permanent, claims);
                }
                else
                {
                    throw new NotImplementedException();
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

        protected virtual Func<IEnumerable<Claim>> BuildSelector(BinaryExpression[] expressions)
        {
            return () =>
            {
                var properties = new List<ClaimProperty>();
                foreach (var expression in expressions)
                {
                    var result = this.Visit(expression);
                    if (result == null)
                    {
                        //Nothing to do.
                    }
                    else if (result is ClaimProperty)
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
                return ClaimProperty.Productize(properties)
                    .Select(group => this.Context.ClaimFactory.Create(group))
                    .Where(claim => claim != null);
            };
        }
    }
}
