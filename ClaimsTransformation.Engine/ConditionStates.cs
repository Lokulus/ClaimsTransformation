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
            this.States = new Dictionary<string, ConditionState>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, ConditionState> States { get; private set; }

        public ConditionState this[string identifier]
        {
            get
            {
                var state = default(ConditionState);
                if (!this.States.TryGetValue(identifier, out state))
                {
                    state = new ConditionState(identifier);
                    this.States.Add(identifier, state);
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
    }

    internal class ConditionState
    {
        public ConditionState(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; private set; }

        public bool IsMatch { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
