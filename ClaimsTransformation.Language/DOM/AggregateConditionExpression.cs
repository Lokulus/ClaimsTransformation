using System.Collections.Generic;

namespace ClaimsTransformation.Language.DOM
{
    public class AggregateConditionExpression : ConditionExpression
    {
        protected AggregateConditionExpression(string identifier, string name, IEnumerable<BinaryExpression> expressions) : base(identifier, expressions)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
