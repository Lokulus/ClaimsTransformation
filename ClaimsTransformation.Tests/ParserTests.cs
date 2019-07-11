using ClaimsTransformation.Language.DOM;
using ClaimsTransformation.Language.Parser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Tests
{
    [TestFixture]
    public class ParserTests
    {
        public IClaimsTransformationParser Parser { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Parser = new ClaimsTransformationParser();
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
                    Enumerable.Empty<ConditionExpression>(),
                    new CopyClaimExpression(IssueDuration.Permanent)
                )
            };
            var actual = this.Parser.Parse(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
