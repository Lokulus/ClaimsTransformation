using System;

namespace ClaimsTransformation.Language.Parser
{
    public class Token
    {
        public Token(string value) : this(value, TokenFlags.Literal)
        {

        }

        public Token(string value, TokenFlags flags)
        {
            this.Value = value;
            this.Flags = flags;
        }

        public string Value { get; private set; }

        public TokenFlags Flags { get; private set; }
    }

    [Flags]
    public enum TokenFlags
    {
        None = 0,
        Literal = 1,
        Identifier = 2,
        Boolean = 4,
        Number = 8,
        String = 16
    }
}
