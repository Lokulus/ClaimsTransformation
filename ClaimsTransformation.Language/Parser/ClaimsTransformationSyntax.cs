namespace ClaimsTransformation.Language.Parser
{
    public static class ClaimsTransformationSyntax
    {
        static ClaimsTransformationSyntax()
        {
            Phase1();
            Phase2();
            Phase3();
        }

        private static void Phase1()
        {
            Property = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.CLAIM, TokenChannel.Normal)
                    ),
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
            ).WithFactory(ExpressionFactory.ClaimProperty);

            Function = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.REGEX_REPLACE, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            AggregateFunction = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.EXISTS, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.NOT_EXISTS, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.COUNT, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            BinaryOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.REGEXP_MATCH, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.REGEXP_NOT_MATCH, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.NEQ, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.EQ, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.ASSIGN, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.CONCAT, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            AggregateOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.EQ, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.GREATER_EQUAL, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.GREATER, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.LESS_EQUAL, TokenChannel.Normal)
                    ),
                    new Syntax(
                        new Token(Terminals.LESS, TokenChannel.Normal)
                    ),
                },
                SyntaxFlags.Any
            );

            String = new Syntax(
                new Token(Terminals.STRING, TokenChannel.Normal, TokenFlags.String)
            );

            Number = new Syntax(
                new Token(Terminals.NUMBER, TokenChannel.Normal, TokenFlags.Number)
            );

            Boolean = new Syntax(
                new Token(Terminals.BOOLEAN, TokenChannel.Normal, TokenFlags.Boolean)
            );

            Identifier = new Syntax(
                new Token(Terminals.IDENTIFIER, TokenChannel.Normal, TokenFlags.Identifier)
            ).WithFactory(ExpressionFactory.Identifier);

            IdentifierProperty = new Syntax(
                new[]
                {
                    Identifier,
                    new Syntax(
                        new Token(Terminals.DOT)
                    ),
                    Property
                },
                SyntaxFlags.All
            ).WithFactory(ExpressionFactory.ConditionProperty);

            Value = new Syntax(
                new[]
                {
                    IdentifierProperty,
                    Property,
                    String,
                    Number,
                    Boolean,
                    Identifier,
                },
                SyntaxFlags.Any
            );
        }

        private static void Phase2()
        {
            Values = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Value,
                            new Syntax(
                                new[]
                                {
                                    new Syntax(
                                        new Token(Terminals.COMMA)
                                    ),
                                    Value
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ),
                    Value
                },
                SyntaxFlags.Any
            );

            Call = new Syntax(
                new[]
                {
                    Function,
                    new Syntax(
                        new Token(Terminals.O_BRACKET)
                    ),
                    Values,
                    new Syntax(
                        new Token(Terminals.C_BRACKET)
                    )
                },
                SyntaxFlags.All
            ).WithFactory(ExpressionFactory.Call);

            Value = new Syntax(
                new[]
                {
                    Call,
                    Value
                },
                SyntaxFlags.Any
            );

            Expression = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Value,
                            new Syntax(
                                new[]
                                {
                                    BinaryOperator,
                                    Value
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ).WithFactory(ExpressionFactory.Binary),
                    Value
                },
                SyntaxFlags.Any
            );

            Expressions = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Expression,
                            new Syntax(
                                new[]
                                {
                                    new Syntax(
                                        new Token(Terminals.COMMA)
                                    ),
                                    Expression
                                },
                                SyntaxFlags.All | SyntaxFlags.Repeat
                            )
                        },
                        SyntaxFlags.All
                    ),
                    Expression
                },
                SyntaxFlags.Any
            );
        }

        private static void Phase3()
        {
            Condition = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new[]
                                {
                                    Identifier,
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
                            Expressions,
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
            ).WithFactory(ExpressionFactory.Condition);

            AggregateCondition = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new[]
                                {
                                    Identifier,
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
                    AggregateFunction,
                    new Syntax(
                        new Token(Terminals.O_BRACKET)
                    ),
                    new Syntax(
                        new Token(Terminals.O_SQ_BRACKET)
                    ),
                    new Syntax(
                        new[]
                        {
                            Expressions,
                            new Syntax(
                                new Token(Terminals.EMPTY)
                            )
                        },
                        SyntaxFlags.Any
                    ),
                    new Syntax(
                        new Token(Terminals.C_SQ_BRACKET)
                    ),
                    new Syntax(
                        new Token(Terminals.C_BRACKET)
                    ),
                    new Syntax(
                        new[]
                        {
                            new Syntax(
                                new[]
                                {
                                    AggregateOperator,
                                    Number
                                },
                                SyntaxFlags.All
                            ),
                            new Syntax(
                                new Token(Terminals.EMPTY)
                            )
                        },
                        SyntaxFlags.Any
                    )
                },
                SyntaxFlags.All
            ).WithFactory(ExpressionFactory.AggregateCondition);

            ConditionOperator = new Syntax(
                new[]
                {
                    new Syntax(
                        new Token(Terminals.AND, TokenChannel.Normal)
                    )
                },
                SyntaxFlags.Any
            );

            Condition = new Syntax(
                new[]
                {
                    Condition,
                    AggregateCondition
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

            Issue = new Syntax(
                new[]
                {
                    new Syntax(
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
                    ),
                    new Syntax(
                        new Token(Terminals.O_BRACKET)
                    ),
                    Expressions,
                    new Syntax(
                        new Token(Terminals.C_BRACKET)
                    )
                },
                SyntaxFlags.All
            ).WithFactory(ExpressionFactory.Issue);

            Rule = new Syntax(
                new[]
                {
                    new Syntax(
                        new[]
                        {
                            Conditions,
                            new Syntax(
                                new Token(Terminals.EMPTY)
                            )
                        },
                        SyntaxFlags.Any
                    ),
                    new Syntax(
                        new Token(Terminals.IMPLY)
                    ),
                    Issue,
                    new Syntax(
                        new Token(Terminals.SEMICOLON)
                    )
                },
                SyntaxFlags.All
            ).WithFactory(ExpressionFactory.Rule);
        }

        public static Syntax Property { get; private set; }

        public static Syntax Function { get; private set; }

        public static Syntax AggregateFunction { get; private set; }

        public static Syntax BinaryOperator { get; private set; }

        public static Syntax AggregateOperator { get; private set; }

        public static Syntax String { get; private set; }

        public static Syntax Number { get; private set; }

        public static Syntax Boolean { get; private set; }

        public static Syntax Identifier { get; private set; }

        public static Syntax IdentifierProperty { get; private set; }

        public static Syntax Value { get; private set; }

        public static Syntax Values { get; private set; }

        public static Syntax Call { get; private set; }

        public static Syntax Expression { get; private set; }

        public static Syntax Expressions { get; private set; }

        public static Syntax Condition { get; private set; }

        public static Syntax AggregateCondition { get; private set; }

        public static Syntax ConditionOperator { get; private set; }

        public static Syntax Conditions { get; private set; }

        public static Syntax Issue { get; private set; }

        public static Syntax Rule { get; private set; }
    }
}
