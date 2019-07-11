using ClaimsTransformation.Language.DOM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ClaimsTransformationEngine : IClaimsTransformationEngine
    {
        public ClaimsTransformationEngine()
        {

        }

        public IEnumerable<Claim> Transform(IEnumerable<RuleExpression> rules, IEnumerable<Claim> claims)
        {
            var context = new ClaimsTransformationContext(claims);
            this.Transform(context);
            return new ReadOnlyCollection<Claim>(context.Output);
        }

        protected virtual void Transform(ClaimsTransformationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
