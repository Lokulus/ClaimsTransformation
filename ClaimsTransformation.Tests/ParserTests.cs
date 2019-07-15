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
        public void Comparison()
        {
            var input = "type == \"http://contoso.com/role\"";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Comparison, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Comparisons()
        {
            var input = "type == \"http://contoso.com/role\", value == \"Editor\", valuetype == \"string\"";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Comparisons, out result));
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
        public void Assignment()
        {
            var input = "value = C1.type + \" \" + C2.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Assignment, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Assignments()
        {
            var input = "type = C1.type, value = C1.type + \" \" + C2.type, valuetype = \"string\"";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Assignments, out result));
            Assert.IsTrue(reader.EOF);
        }

        [Test]
        public void Issue_New()
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
            var input = new[]
            {
                "C1:[] => Issue(claim = C1);"
            };
            var expected = new[]
            {
                new RuleExpression(
                    new[]
                    {
                        new ConditionExpression("C1", Enumerable.Empty<BinaryExpression>())
                    },
                    new CopyClaimExpression(IssueDuration.Permanent, "C1")
                )
            };
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Test002()
        {
            var input = new[]
            {
                "C1:[type == \"http://contoso.com/role\", value == \"Editor\"] && " +
                "C2:[type == \"http://contoso.com/role\", value == \"Manager\"] " +
                "=> Issue(type = C1.type, value = C1.type + \" \" + C2.type, valuetype = \"string\");"
            };
            var expected = new[]
            {
                new RuleExpression(
                    Enumerable.Empty<ConditionExpression>(),
                    new CopyClaimExpression(IssueDuration.Permanent, "C1")
                )
            };
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
