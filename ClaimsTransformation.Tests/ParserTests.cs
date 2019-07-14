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

        [Test]
        public void Test001()
        {
            var input = "C1:[type == \"http://contoso.com/role\", value == \"Editor\"]";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Condition, out result));
        }

        [Test]
        public void Test002()
        {
            var input =
                "C1:[type == \"http://contoso.com/role\", value == \"Editor\"] && " +
                "C2:[type == \"http://contoso.com/role\", value == \"Manager\"]";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Conditions, out result));
        }

        [Test]
        public void Test003()
        {
            var input ="Issue(claim = C1)";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Issue, out result));
        }

        [Test]
        public void Test004()
        {
            var input = "C1.type + \" \" + C2.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Expression, out result));
        }

        [Test]
        public void Test005()
        {
            var input = "value = C1.type + \" \" + C2.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Assignment, out result));
        }

        [Test]
        public void Test006()
        {
            var input = "type = C1.type, value = C1.type + \" \" + C2.type";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Assignments, out result));
        }

        [Test]
        public void Test007()
        {
            var input = "Issue(type = C1.type, value = C1.type + \" \" + C2.type)";
            var reader = new StringReader(input);
            var result = default(TokenValue);
            Assert.IsTrue(this.Parser.TryParse(reader, ClaimsTransformationSyntax.Issue, out result));
        }

        [Test]
        public void Test101()
        {
            var input = new[]
            {
                "C1:[] => Issue(claim = C1);"
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


        [Test]
        public void Test102()
        {
            var input = new[]
            {
                "C1:[type == \"http://contoso.com/role\", value == \"Editor\"] && " +
                "C2:[type == \"http://contoso.com/role\", value == \"Manager\"] " +
                "=> Issue(type = C1.type, value = C1.type + \" \" + C2.type);"
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
