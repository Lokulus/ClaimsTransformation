using ClaimsTransformation.Language.DOM;
using System;
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

        public ConditionState this[ExpressionVisitor visitor, string identifier]
        {
            get
            {
                var expression = this.States.Keys.FirstOrDefault(
                    _expression => string.Equals(
                        Convert.ToString(visitor.Visit(_expression.Identifier)),
                        identifier,
                        StringComparison.OrdinalIgnoreCase
                    )
                );
                return this[expression];
            }
        }

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

        public IEnumerable<ConditionExpression> Expressions
        {
            get
            {
                return this.States.Keys;
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                return this.States.Values.SelectMany(state => state.Claims).ToArray();
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
