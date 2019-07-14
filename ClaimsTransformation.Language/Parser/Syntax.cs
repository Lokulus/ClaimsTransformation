using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsTransformation.Language.Parser
{
    public class Syntax
    {
        public Syntax(Token token) : this(token, Enumerable.Empty<Syntax>(), SyntaxFlags.None)
        {

        }

        public Syntax(IEnumerable<Syntax> children, SyntaxFlags flags) : this(null, children, flags)
        {

        }

        public Syntax(Token token, IEnumerable<Syntax> children, SyntaxFlags flags)
        {
            this.Token = token;
            this.Children = children;
            this.Flags = flags;
        }

        public Token Token { get; private set; }

        public IEnumerable<Syntax> Children { get; private set; }

        public SyntaxFlags Flags { get; private set; }
    }

    [Flags]
    public enum SyntaxFlags
    {
        None = 0,
        Any = 1,
        All = 2,
        Repeat = 4
    }
}
