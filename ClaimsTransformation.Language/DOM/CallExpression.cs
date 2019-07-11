using System.Collections.Generic;

namespace ClaimsTransformation.Language.DOM
{
    public class CallExpression : Expression
    {
        public CallExpression(string name, IEnumerable<Expression> arguments)
        {
            this.Name = name;
            this.Arguments = arguments;
        }

        public string Name { get; private set; }

        public IEnumerable<Expression> Arguments { get; private set; }
    }
}
