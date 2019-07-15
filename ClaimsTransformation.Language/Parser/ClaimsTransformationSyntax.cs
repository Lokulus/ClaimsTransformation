namespace ClaimsTransformation.Language.Parser
{
    public static class ClaimsTransformationSyntax
    {
        static ClaimsTransformationSyntax()
        {
            Property = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.TYPE)
                    ),
                    new Syntax(
                        new Token(Terminals.VALUE)
                    ),
                    new Syntax(
                        new Token(Terminals.VALUE_TYPE)
                    )
                },
                SyntaxFlags.Any
            );

            BooleanOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.EQ)
                    ),
                    new Syntax(
                        new Token(Terminals.NEQ)
                    ),
                    new Syntax(
                        new Token(Terminals.REGEXP_MATCH)
                    ),
                    new Syntax(
                        new Token(Terminals.REGEXP_NOT_MATCH)
                    )
                },
                SyntaxFlags.Any
            );

            Value = new Syntax(
                new Token(Terminals.STRING, TokenFlags.String)
            );

            ValueOrProperty = new Syntax(
                new[]
                {
                    Value,
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new Token(Terminals.IDENTIFIER, TokenFlags.Identifier)
                            ),
                            new Syntax(
                                new Token(Terminals.DOT)
                            ),
                            Property
                        },
                        SyntaxFlags.All
                    )
                },
                SyntaxFlags.Any
            );

            ExpressionOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.CONCAT)
                    )
                },
                SyntaxFlags.Any
            );

            Expression = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            ValueOrProperty,
                            new Syntax(
                                new[]
                                {
                                    ExpressionOperator,
                                    ValueOrProperty
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ),
                    ValueOrProperty
                },
                SyntaxFlags.Any
            );

            Comparison = new Syntax(
                new[]
                {
                    Property,
                    BooleanOperator,
                    Value
                },
                SyntaxFlags.All
            );

            Comparisons = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Comparison,
                            new Syntax(
                                new[]
                                {
                                    new Syntax(
                                        new Token(Terminals.COMMA)
                                    ),
                                    Comparison
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ),
                    Comparison
                },
                SyntaxFlags.Any
            );

            Condition = new Syntax(
                new[]
                {
                    new Syntax(
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
                                },
                                SyntaxFlags.All
                            ),
                            new Syntax(
                                new Token(Terminals.EMPTY)
                            )
                        },
                        SyntaxFlags.Any
                    ),
                    new Syntax(
                        new Token(Terminals.O_SQ_BRACKET)
                    ),
                    new Syntax(
                        new[]
                        {
                            Comparisons,
                            new Syntax(
                                new Token(Terminals.EMPTY)
                            )
                        },
                        SyntaxFlags.Any
                    ),
                    new Syntax(
                        new Token(Terminals.C_SQ_BRACKET)
                    )
                },
                SyntaxFlags.All
            );

            Conditions = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Condition,
                            new Syntax(
                                new[]
                                {
                                    new Syntax(
                                        new Token(Terminals.AND)
                                    ),
                                    Condition
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ),
                    Condition,
                },
                SyntaxFlags.Any
            );

            Copy = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.CLAIM)
                    ),
                    new Syntax(
                        new Token(Terminals.ASSIGN)
                    ),
                    new Syntax(
                        new Token(Terminals.IDENTIFIER, TokenFlags.Identifier)
                    ),
                },
                SyntaxFlags.All
            );

            Assignment = new Syntax(
                new[]
                {
                    Property,
                    new Syntax(
                        new Token(Terminals.ASSIGN)
                    ),
                    Expression
                },
                SyntaxFlags.All
            );

            Assignments = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Assignment,
                            new Syntax(
                                new[]
                                {
                                    new Syntax(
                                        new Token(Terminals.COMMA)
                                    ),
                                    Assignment
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ),
                    Assignment,
                },
                SyntaxFlags.Any
            );

            Create = new Syntax(
                new[]
                {
                    Assignments
                },
                SyntaxFlags.All
            );

            Issue = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.ISSUE)
                    ),
                    new Syntax(
                        new Token(Terminals.O_BRACKET)
                    ),
                    new Syntax(
                        new[]
                        {
                            Copy,
                            Create
                        },
                        SyntaxFlags.Any
                    ),
                    new Syntax(
                        new Token(Terminals.C_BRACKET)
                    )
                },
                SyntaxFlags.All
            );

            Rule = new Syntax(
                new[]
                {
                    Conditions,
                    new Syntax(
                        new Token(Terminals.IMPLY)
                    ),
                    Issue,
                    new Syntax(
                        new Token(Terminals.SEMICOLON)
                    )
                },
                SyntaxFlags.All
            );
        }

        public static Syntax Property { get; private set; }

        public static Syntax BooleanOperator { get; private set; }

        public static Syntax Value { get; private set; }

        public static Syntax ValueOrProperty { get; private set; }

        public static Syntax ExpressionOperator { get; private set; }

        public static Syntax Expression { get; private set; }

        public static Syntax Comparison { get; private set; }

        public static Syntax Comparisons { get; private set; }

        public static Syntax Condition { get; private set; }

        public static Syntax Conditions { get; private set; }

        public static Syntax Assignment { get; private set; }

        public static Syntax Assignments { get; private set; }

        public static Syntax Copy { get; private set; }

        public static Syntax Create { get; private set; }

        public static Syntax Issue { get; private set; }

        public static Syntax Rule { get; private set; }
    }
}
