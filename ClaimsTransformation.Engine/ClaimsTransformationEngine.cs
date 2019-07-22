using ClaimsTransformation.Language.DOM;
using ClaimsTransformation.Language.Parser;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ClaimsTransformationEngine : IClaimsTransformationEngine
    {
        public ClaimsTransformationEngine(IClaimsTransformationParser parser, IClaimsTransformationCache cache)
        {
            this.Parser = parser;
            this.Cache = cache;
        }

        public IClaimsTransformationParser Parser { get; private set; }

        public IClaimsTransformationCache Cache { get; private set; }

        public IEnumerable<Claim> Transform(string rule, IEnumerable<Claim> claims)
        {
            return this.Transform(new[] { rule }, claims);
        }

        public IEnumerable<Claim> Transform(IEnumerable<string> rules, IEnumerable<Claim> claims)
        {
            var context = new ClaimsTransformationContext(claims);
            this.Transform(rules, context);
            return context.Output;
        }

        protected virtual void Transform(IEnumerable<string> rules, ClaimsTransformationContext context)
        {
            foreach (var rule in rules)
            {
                var expression = default(RuleExpression);
                if (!this.Cache.TryGetValue(rule, out expression))
                {
                    expression = this.Parser.Parse(rule);
                    this.Cache.Add(rule, expression);
                }
                this.Transform(expression, context);
            }
        }

        protected virtual void Transform(RuleExpression expression, ClaimsTransformationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
