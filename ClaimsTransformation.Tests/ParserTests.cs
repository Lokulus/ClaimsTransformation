using ClaimsTransformation.Language.DOM;
using ClaimsTransformation.Language.Parser;
using NUnit.Framework;
using System.Linq;

namespace ClaimsTransformation.Tests
{
    [TestFixture]
    public class ParserTests
    {
        public ClaimsTransformationParser Parser { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Parser = new ClaimsTransformationParser();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void Property()
        {
            var input = "type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Property, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void IdentifierProperty()
        {
            var input = "C1.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.IdentifierProperty, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Comparison()
        {
            var input = "type == \"http://contoso.com/role\"";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Expression, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Comparisons()
        {
            var input = "type == \"http://contoso.com/role\", value == \"Editor\", valuetype == \"string\"";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Expressions, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Condition()
        {
            var input = "C1:[type == \"http://contoso.com/role\", value == \"Editor\"]";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Condition, out result));
            Assert.IsTrue(reader.EOF);
        }

        [TestCase("C1:EXISTS([type == \"http://contoso.com/role\", value == \"Editor\"])")]
        [TestCase("C1:NOT EXISTS([type == \"http://contoso.com/role\", value == \"Editor\"])")]
        [TestCase("C1:COUNT([type == \"http://contoso.com/role\", value == \"Editor\"]) > 10")]
        public void AggregateCondition(string input)
        {
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Condition, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Conditions()
        {
            var input =
                "C1:[type == \"http://contoso.com/role\", value == \"Editor\"] && " +
                "C2:[type == \"http://contoso.com/role\", value == \"Manager\"]";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Conditions, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Issue_Copy()
        {
            var input = "Issue(claim = C1)";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Issue, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Expression()
        {
            var input = "C1.type + \" \" + C2.type + \" \" + C3.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Expression, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Call()
        {
            var input = "RegExReplace(C1.value, \"Cats\", \"Dogs\")";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Call, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Assignment()
        {
            var input = "value = C1.type + \" \" + C2.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Expression, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Assignments()
        {
            var input = "type = C1.type, value = C1.type + \" \" + C2.type, valuetype = \"string\"";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Expressions, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Issue_Create()
        {
            var input = "Issue(type = C1.type, value = C1.type + \" \" + C2.type)";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Issue, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Test001()
        {
            var input = "C1:[] => Issue(claim = C1);";
            var expected = new RuleExpression(
                new[]
                {
                    new ConditionExpression(
                        new LiteralExpression("C1"),
                        Enumerable.Empty<BinaryExpression>()
                    )
                },
                new IssueExpression(
                    new LiteralExpression("Issue"),
                    new[]
                    {
                        new BinaryExpression(
                            new ClaimPropertyExpression("claim"),
                            new LiteralExpression("="),
                            new LiteralExpression("C1")
                        )
                    }
                )
            );
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Test002()
        {
            var input =
                "C1:[type == \"http://contoso.com/role\", value == \"Editor\"] && " +
                "C2:[type == \"http://contoso.com/role\", value == \"Manager\"] " +
                "=> Issue(type = C1.type, value = C1.type + \" \" + C2.type, valuetype = \"string\");";
            var expected = new RuleExpression(
                new[]
                {
                    new ConditionExpression(
                        new LiteralExpression("C1"),
                        new[]
                        {
                            new BinaryExpression(
                                new ClaimPropertyExpression("type"),
                                new LiteralExpression("=="),
                                new LiteralExpression("http://contoso.com/role")
                            ),
                            new BinaryExpression(
                                new ClaimPropertyExpression("value"),
                                new LiteralExpression("=="),
                                new LiteralExpression("Editor")
                            )
                        }
                    ),
                    new ConditionExpression(
                        new LiteralExpression("C2"),
                        new[]
                        {
                            new BinaryExpression(
                                new ClaimPropertyExpression("type"),
                                new LiteralExpression("=="),
                                new LiteralExpression("http://contoso.com/role")
                            ),
                            new BinaryExpression(
                                new ClaimPropertyExpression("value"),
                                new LiteralExpression("=="),
                                new LiteralExpression("Manager")
                            )
                        }
                    )
                },
                new IssueExpression(
                    new LiteralExpression("Issue"),
                    new[]
                    {
                        new BinaryExpression(
                            new ClaimPropertyExpression("type"),
                            new LiteralExpression("="),
                            new ConditionPropertyExpression(
                                new LiteralExpression("C1"),
                                new ClaimPropertyExpression("type")
                            )
                        ),
                        new BinaryExpression(
                            new ClaimPropertyExpression("value"),
                            new LiteralExpression("="),
                            new BinaryExpression(
                                new BinaryExpression(
                                    new ConditionPropertyExpression(
                                        new LiteralExpression("C1"),
                                        new ClaimPropertyExpression("type")
                                    ),
                                    new LiteralExpression("+"),
                                    new LiteralExpression(" ")
                                ),
                                new LiteralExpression("+"),
                                new ConditionPropertyExpression(
                                    new LiteralExpression("C2"),
                                    new ClaimPropertyExpression("type")
                                )
                            )
                        ),
                        new BinaryExpression(
                            new ClaimPropertyExpression("valuetype"),
                            new LiteralExpression("="),
                            new LiteralExpression("string")
                        )
                    }
                )
            );
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test003()
        {
            var input = "C1:[] => Issue(value = RegExReplace(C1.value, \"Cats\", \"Dogs\"));";
            var expected = new RuleExpression(
                new[]
                {
                    new ConditionExpression(
                        new LiteralExpression("C1"),
                        Enumerable.Empty<BinaryExpression>()
                    )
                },
                new IssueExpression(
                    new LiteralExpression("Issue"),
                    new[]
                    {
                        new BinaryExpression(
                            new ClaimPropertyExpression("value"),
                            new LiteralExpression("="),
                            new CallExpression(
                                new LiteralExpression("RegExReplace"),
                                new Expression[]
                                {
                                    new ConditionPropertyExpression(
                                        new LiteralExpression("C1"),
                                        new ClaimPropertyExpression("value")
                                    ),
                                    new LiteralExpression("Cats"),
                                    new LiteralExpression("Dogs")
                                }
                            )
                        )
                    }
                )
            );
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test004()
        {
            var input = "C1:EXISTS([type == \"http://contoso.com/role\", value == \"Manager\"]) => Issue(claim = C1);";
            var expected = new RuleExpression(
                new[]
                {
                    new AggregateConditionExpression(
                        new LiteralExpression("C1"),
                        new LiteralExpression("EXISTS"),
                        new[]
                        {
                            new BinaryExpression(
                                new ClaimPropertyExpression("type"),
                                new LiteralExpression("=="),
                                new LiteralExpression("http://contoso.com/role")
                            ),
                            new BinaryExpression(
                                new ClaimPropertyExpression("value"),
                                new LiteralExpression("=="),
                                new LiteralExpression("Manager")
                            )
                        }
                    )
                },
                new IssueExpression(
                    new LiteralExpression("Issue"),
                        new[]
                    {
                        new BinaryExpression(
                            new ClaimPropertyExpression("claim"),
                            new LiteralExpression("="),
                            new LiteralExpression("C1")
                        )
                    }
                )
            );
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test005()
        {
            var input = "C1:COUNT([type == \"http://contoso.com/role\", value == \"Manager\"]) == 1 => Issue(claim = C1);";
            var expected = new RuleExpression(
                new[]
                {
                    new AggregateConditionExpression(
                        new LiteralExpression("C1"),
                        new LiteralExpression("COUNT"),
                        new[]
                        {
                            new BinaryExpression(
                                new ClaimPropertyExpression("type"),
                                new LiteralExpression("=="),
                                new LiteralExpression("http://contoso.com/role")
                            ),
                            new BinaryExpression(
                                new ClaimPropertyExpression("value"),
                                new LiteralExpression("=="),
                                new LiteralExpression("Manager")
                            )
                        },
                        new LiteralExpression("=="),
                        new LiteralExpression("1")
                    )
                },
                new IssueExpression(
                    new LiteralExpression("Issue"),
                        new[]
                    {
                        new BinaryExpression(
                            new ClaimPropertyExpression("claim"),
                            new LiteralExpression("="),
                            new LiteralExpression("C1")
                        )
                    }
                )
            );
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
