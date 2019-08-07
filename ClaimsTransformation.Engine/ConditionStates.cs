using ClaimsTransformation.Language.DOM;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    internal class ConditionStates
    {
        public ConditionStates()
        {
            this.States = new Dictionary<ConditionExpression, ConditionState>();
        }

        public IDictionary<ConditionExpression, ConditionState> States { get; private set; }

        public ConditionState this[ConditionExpression expression]
        {
            get
            {
                var state = default(ConditionState);
                if (!this.States.TryGetValue(expression, out state))
                {
                    state = new ConditionState(expression);
                    this.States.Add(expression, state);
                }
                return state;
            }
        }

        public bool IsMatch
        {
            get
            {
                return this.States.Values.All(state => state.IsMatch);
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                var claims = default(IEnumerable<Claim>);
                foreach (var state in this.States.Values)
                {
                    if (claims == null)
                    {
                        claims = state.Claims;
                    }
                    else
                    {
                        claims = claims.Intersect(state.Claims);
                    }
                }
                return claims;
            }
        }
    }

    internal class ConditionState
    {
        public ConditionState(ConditionExpression condition)
        {
            this.Condition = condition;
        }

        public ConditionExpression Condition { get; private set; }

        public bool IsMatch { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
