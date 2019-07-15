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
                        new Token(Terminals.TYPE, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.VALUE_TYPE, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.VALUE, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            BooleanOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.EQ, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.NEQ, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.REGEXP_MATCH, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.REGEXP_NOT_MATCH, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            Value = new Syntax(
                new Token(Terminals.STRING, TokenChannel.Normal, TokenFlags.String)
            );

            ValueOrProperty = new Syntax(
                new[]
                {
                    Value,
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new Token(Terminals.IDENTIFIER, TokenChannel.Normal, TokenFlags.Identifier)
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
                        new Token(Terminals.CONCAT, TokenChannel.Normal)
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
                                        new Token(Terminals.IDENTIFIER, TokenChannel.Normal, TokenFlags.Identifier)
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

            ConditionOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.AND, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
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
                                    ConditionOperator,
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
                        new Token(Terminals.IDENTIFIER, TokenChannel.Normal, TokenFlags.Identifier)
                    ),
                },
                SyntaxFlags.All
            );

            Assignment = new Syntax(
                new[]
                {
                    Property,
                    new Syntax(
                        new Token(Terminals.ASSIGN, TokenChannel.Normal)
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

            Issuance = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.ISSUE, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.ADD, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            Issue = new Syntax(
                new[]
                {
                    Issuance,
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

        public static Syntax ConditionOperator { get; private set; }

        public static Syntax Conditions { get; private set; }

        public static Syntax Assignment { get; private set; }

        public static Syntax Assignments { get; private set; }

        public static Syntax Copy { get; private set; }

        public static Syntax Create { get; private set; }

        public static Syntax Issuance { get; private set; }

        public static Syntax Issue { get; private set; }

        public static Syntax Rule { get; private set; }
    }
}
