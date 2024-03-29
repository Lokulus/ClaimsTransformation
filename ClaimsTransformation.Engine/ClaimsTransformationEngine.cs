﻿using ClaimsTransformation.Language.DOM;
using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ClaimsTransformationEngine : IClaimsTransformationEngine
    {
        public ClaimsTransformationEngine(IClaimsTransformationParser parser, IClaimsTransformationCache cache, IClaimFactory claimFactory, ClaimsTransformationFlags flags = ClaimsTransformationFlags.None)
        {
            this.Parser = parser;
            this.Cache = cache;
            this.ClaimFactory = claimFactory;
            this.Flags = flags;
        }

        public IClaimsTransformationParser Parser { get; private set; }

        public IClaimsTransformationCache Cache { get; private set; }

        public IClaimFactory ClaimFactory { get; private set; }

        public ClaimsTransformationFlags Flags { get; private set; }

        public IEnumerable<Claim> Transform(string rule, IEnumerable<Claim> claims)
        {
            return this.Transform(new[] { rule }, claims);
        }

        public IEnumerable<Claim> Transform(IEnumerable<string> rules, IEnumerable<Claim> claims)
        {
            var context = new ClaimsTransformationContext(this.ClaimFactory, claims, this.Flags);
            this.Transform(rules, context);
            return context.Output;
        }

        protected virtual void Transform(IEnumerable<string> rules, IClaimsTransformationContext context)
        {
            foreach (var rule in rules)
            {
                if (string.IsNullOrEmpty(rule))
                {
                    continue;
                }
                var expression = default(RuleExpression);
                if (!this.Cache.TryGetValue(rule, out expression))
                {
                    expression = this.Parser.Parse(rule);
                    this.Cache.Add(rule, expression);
                }
                this.Transform(expression, context);
            }
        }

        protected virtual void Transform(RuleExpression expression, IClaimsTransformationContext context)
        {
            var visitor = new ExpressionVisitor(context);
            visitor.Visit(expression);
        }
    }
}
