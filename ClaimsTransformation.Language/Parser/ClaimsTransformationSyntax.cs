namespace ClaimsTransformation.Language.Parser
{
    public static class ClaimsTransformationSyntax
    {
        static ClaimsTransformationSyntax()
        {
            Syntax = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new Token(Terminals.IDENTIFIER, TokenFlags.Identifier)
                            ),
                            new Syntax(
                                new Token(Terminals.COLON)
                            ),
                            new Syntax(
                                new Token(Terminals.O_SQ_BRACKET)
                            ),
                            new Syntax(
                                new Token(Terminals.C_SQ_BRACKET)
                            )
                        },
                        SyntaxFlags.All
                    ),
                    new Syntax(
                        new Token(Terminals.IMPLY)
                    ),
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new Token(Terminals.ISSUE)
                            ),
                            new Syntax(
                                new Token(Terminals.O_BRACKET)
                            ),
                            new Syntax(
                                new Token(Terminals.CLAIM)
                            ),
                            new Syntax(
                                new Token(Terminals.ASSIGN)
                            ),
                            new Syntax(
                                new Token(Terminals.IDENTIFIER, TokenFlags.Identifier)
                            ),
                            new Syntax(
                                new Token(Terminals.C_BRACKET)
                            )
                        },
                        SyntaxFlags.All
                    ),
                    new Syntax(
                        new Token(Terminals.SEMICOLON)
                    )
                },
                SyntaxFlags.All
            );
        }

        public static Syntax Syntax { get; private set; }
    }
}
