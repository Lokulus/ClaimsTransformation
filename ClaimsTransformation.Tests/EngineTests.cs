using ClaimsTransformation.Engine;
using ClaimsTransformation.Language.Parser;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsTransformation.Tests
{
    [TestFixture]
    public class EngineTests
    {
        public Utility Utility { get; private set; }

        public ClaimsTransformationParser Parser { get; private set; }

        public ClaimsTransformationCache Cache { get; private set; }

        public ClaimsTransformationEngine Engine { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Utility = new Utility();
            this.Parser = new ClaimsTransformationParser();
            this.Cache = new ClaimsTransformationCache();
            this.Engine = new ClaimsTransformationEngine(this.Parser, this.Cache);
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void Scenario001()
        {
            const string EXPRESSION =
                @"C1:[] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashPublic"");";

            var positive = new List<Claim>();

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashPublic"));
        }

        /// <summary>
        /// Assign a subject to a group.
        /// </summary>
        [Test]
        public void Scenario002()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/subject", "aidang"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Groups within groups.
        /// </summary>
        [Test]
        public void Scenario003()
        {
            const string EXPRESSION_1 =
                @"C1:[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""Developers"");";
            const string EXPRESSION_2 =
                @"C2:[TYPE == ""http://namespace/role"", VALUE == ""Developers""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(new[] { EXPRESSION_1, EXPRESSION_2 }, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "Developers"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/subject", "aidang"));

            var fail = this.Engine.Transform(new[] { EXPRESSION_1, EXPRESSION_2 }, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "Developers"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Non-impersonated client.
        /// </summary>
        [Test]
        public void Scenario004()
        {
            const string EXPRESSION =
                @"C1:EXISTS[TYPE == ""http://namespace/client"", VALUE == ""CodeApi""] && " +
                @"C2:NOT EXISTS[TYPE == ""http://namespace/subject"", VALUE == ""Scenario004""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "CodeApi"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/client", "CodeApi"));
            negative.Add(new Claim("http://namespace/subject", "Scenario004"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Subject from specific client.
        /// </summary>
        [Test]
        public void Scenario005()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client"", VALUE == ""WebApp""] && " +
                @"C2:[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ISSUE(TYPE = ""http://namespace/clientsubject"", VALUE = ""WebApp_robh"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "WebApp"));
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/clientsubject", "WebApp_robh"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/client", "WebApp"));
            negative.Add(new Claim("http://namespace/subject", "aidang"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/clientsubject", "WebApp_robh"));
        }

        /// <summary>
        /// Concatenated value.
        /// </summary>
        [Test]
        public void Scenario006()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client"", VALUE == ""WebApp""] && " +
                @"C2:[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ISSUE(TYPE = ""http://namespace/clientsubject"", VALUE = C1.VALUE + ""_"" + C2.VALUE);";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "WebApp"));
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/clientsubject", "WebApp_robh"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/client", "WebApp"));
            negative.Add(new Claim("http://namespace/subject", "aidang"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/clientsubject", "WebApp_robh"));
        }

        /// <summary>
        /// Claim or.
        /// </summary>
        [Test]
        public void Scenario007()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client"", VALUE == ""Test001""] || " +
                @"C2:[TYPE == ""http://namespace/client"", VALUE == ""Test002""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""Test003"");";

            var positive1 = new List<Claim>();
            positive1.Add(new Claim("http://namespace/client", "Test001"));

            var pass1 = this.Engine.Transform(EXPRESSION, positive1);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass1, "http://namespace/role", "Test003"));

            var positive2 = new List<Claim>();
            positive2.Add(new Claim("http://namespace/client", "Test002"));

            var pass2 = this.Engine.Transform(EXPRESSION, positive2);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass2, "http://namespace/role", "Test003"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/client", "Test004"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "Test003"));
        }

        /// <summary>
        /// Transient claim.
        /// </summary>
        [Test]
        public void Scenario008()
        {
            const string EXPRESSION_1 =
                @"C1:[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ADD(TYPE = ""http://namespace/role"", VALUE = ""Developers"");";
            const string EXPRESSION_2 =
                @"C2:[TYPE == ""http://namespace/role"", VALUE == ""Developers""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(new[] { EXPRESSION_1, EXPRESSION_2 }, positive);

            Assert.IsFalse(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "Developers")); //Transient claim should not be present.
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Issue multiple.
        /// </summary>
        [Test]
        public void Scenario009()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client""] " +
                @"=> ISSUE(TYPE = ""http://namespace/client"", VALUE = ""Issue_"" + C1.VALUE);";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "Test001"));
            positive.Add(new Claim("http://namespace/client", "Test002"));
            positive.Add(new Claim("http://namespace/client", "Test003"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test001"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test002"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test003"));

            var negative = new List<Claim>();

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test001"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test002"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test003"));
        }

        /// <summary>
        /// Copy multiple.
        /// </summary>
        [Test]
        public void Scenario010()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client""] " +
                @"=> ISSUE(CLAIM = C1);";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "Test001"));
            positive.Add(new Claim("http://namespace/client", "Test002"));
            positive.Add(new Claim("http://namespace/client", "Test003"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Test001"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Test002"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Test003"));

            var negative = new List<Claim>();

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Test001"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Test002"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Test003"));
        }

        /// <summary>
        /// Issue multiple complex.
        /// </summary>
        [Test]
        public void Scenario011()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client""] && " +
                @"C2:[VALUE == ""Test002""] " +
                @"=> ISSUE(TYPE = ""http://namespace/client"", VALUE = ""Issue_"" + C2.VALUE);";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "Test001"));
            positive.Add(new Claim("http://namespace/client", "Test002"));
            positive.Add(new Claim("http://namespace/client", "Test003"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsFalse(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test001"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test002"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test003"));

            var negative = new List<Claim>();

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test001"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test002"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test003"));
        }

        /// <summary>
        /// Client and at least 3 values starting with Test.
        /// </summary>
        [Test]
        public void Scenario012()
        {
            const string EXPRESSION =
                @"C1:[TYPE == ""http://namespace/client""] && " +
                @"C2:COUNT([VALUE =~ ""Test""]) >= 3 " +
                @"=> ISSUE(TYPE = ""http://namespace/client"", VALUE = ""Issue_"" + C2.VALUE);";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/client", "Test001"));
            positive.Add(new Claim("http://namespace/client", "Test002"));
            positive.Add(new Claim("http://namespace/client", "Test003"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test001"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test002"));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/client", "Issue_Test003"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/client", "Test001"));
            negative.Add(new Claim("http://namespace/client", "Test002"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test001"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test002"));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/client", "Issue_Test003"));
        }

        /// <summary>
        /// Text values containing dots.
        /// </summary>
        [Test]
        public void Scenario013()
        {
            const string EXPRESSION =
                @"C1: [TYPE == ""role""] => ISSUE(TYPE = ""http://schemas.microsoft.com/ws/2008/06/identity/claims/role"", VALUE = C1.VALUE);";

            var positive = new List<Claim>();
            positive.Add(new Claim("role", "admin"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin"));

            var negative = new List<Claim>();

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(negative, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin"));
        }

        /// <summary>
        ///  Ensure duplicate claims aren't created.
        /// </summary>
        [Test]
        public void Scenario014()
        {
            const string EXPRESSION_1 =
               @"C1: [TYPE == ""sub"", VALUE == ""D44D4E00-D914-400A-A97D-B9DD0A50AA9B""] => ISSUE(TYPE = ""role"", VALUE = ""admin"");";
            const string EXPRESSION_2 =
                @"C1: EXISTS([]) => ISSUE(TYPE = ""role"", VALUE = ""anonymous"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("client_id", "codify"));
            positive.Add(new Claim("scope", "openid"));
            positive.Add(new Claim("scope", "api"));
            positive.Add(new Claim("scope", "api.codeapi"));
            positive.Add(new Claim("scope", "name"));
            positive.Add(new Claim("scope", "roles"));
            positive.Add(new Claim("sub", "d44d4e00-d914-400a-a97d-b9dd0a50aa9b"));
            positive.Add(new Claim("amr", "password"));
            positive.Add(new Claim("auth_time", "1438772490"));
            positive.Add(new Claim("idp", "idsrv"));
            positive.Add(new Claim("name", "Default Administrator"));
            positive.Add(new Claim("role", "ego.RoleAdmin"));
            positive.Add(new Claim("role", "ego.UserAdmin"));
            positive.Add(new Claim("role", "ego.UserClaimAdmin"));
            positive.Add(new Claim("role", "ego.UserRoleAdmin"));
            positive.Add(new Claim("exp", "1438774167"));
            positive.Add(new Claim("nbf", "1438773807"));

            var pass = this.Engine.Transform(new[] { EXPRESSION_1, EXPRESSION_2 }, positive);

            Assert.IsTrue(this.Utility.GetIssuedClaims(pass, "role", "admin").Count() == 1);
            Assert.IsTrue(this.Utility.GetIssuedClaims(pass, "role", "anonymous").Count() == 1);

        }

        /// <summary>
        ///  Empty rule performs operation for *all* claims however duplicates are not issued.
        /// </summary>
        [Test]
        public void Scenario015()
        {
            const string EXPRESSION =
                @"C1: [] => ISSUE(TYPE = ""role"", VALUE = ""anonymous"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("client_id", "codify"));
            positive.Add(new Claim("scope", "openid"));
            positive.Add(new Claim("scope", "api"));
            positive.Add(new Claim("scope", "api.codeapi"));
            positive.Add(new Claim("scope", "name"));
            positive.Add(new Claim("scope", "roles"));
            positive.Add(new Claim("sub", "d44d4e00-d914-400a-a97d-b9dd0a50aa9b"));
            positive.Add(new Claim("amr", "password"));
            positive.Add(new Claim("auth_time", "1438772490"));
            positive.Add(new Claim("idp", "idsrv"));
            positive.Add(new Claim("name", "Default Administrator"));
            positive.Add(new Claim("role", "ego.RoleAdmin"));
            positive.Add(new Claim("role", "ego.UserAdmin"));
            positive.Add(new Claim("role", "ego.UserClaimAdmin"));
            positive.Add(new Claim("role", "ego.UserRoleAdmin"));
            positive.Add(new Claim("exp", "1438774167"));
            positive.Add(new Claim("nbf", "1438773807"));

            var pass = this.Engine.Transform(EXPRESSION, positive);
            Assert.IsTrue(this.Utility.GetIssuedClaims(pass, "role", "anonymous").Count() == 1);
        }

        /// <summary>
        /// Identifier should be optional.
        /// </summary>
        [Test]
        public void Scenario016()
        {
            const string EXPRESSION =
                @"[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/subject", "aidang"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Conditions should be optional.
        /// </summary>
        [Test]
        public void Scenario017()
        {
            const string EXPRESSION =
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Conditions should be optional.
        /// </summary>
        [Test]
        public void Scenario018()
        {
            const string EXPRESSION =
                @"C1: [] => ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// Entire expression should be optional.
        /// </summary>
        [Test]
        public void Scenario019()
        {
            const string EXPRESSION =
                @"";

            var positive = new List<Claim>();

            var pass = this.Engine.Transform(EXPRESSION, positive);
        }

        /// <summary>
        /// Entire expression should be optional.
        /// </summary>
        [Test]
        public void Scenario020()
        {
            const string EXPRESSION =
                @";";

            var positive = new List<Claim>();

            var pass = this.Engine.Transform(EXPRESSION, positive);
        }

        /// <summary>
        /// Ensure original (input) claims are not present in the result.
        /// </summary>
        [Test]
        public void Scenario021()
        {
            const string EXPRESSION =
                @"[TYPE == ""http://namespace/subject"", VALUE == ""robh""] " +
                @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""StashAdmin"");";

            var positive = new List<Claim>();
            positive.Add(new Claim("http://namespace/subject", "robh"));

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsFalse(pass.Contains(new Claim("http://namespace/subject", "robh")));
            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://namespace/role", "StashAdmin"));

            var negative = new List<Claim>();
            negative.Add(new Claim("http://namespace/subject", "aidang"));

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(pass.Contains(new Claim("http://namespace/subject", "robh")));
            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://namespace/role", "StashAdmin"));
        }

        /// <summary>
        /// User has role of editors and an email.
        /// </summary>
        [Test]
        public void Scenario023()
        {
            const string EXPRESSION =
                @"c1:[Type == ""http://contoso.com/role"", Value==""Editors""] && " +
                @"c2:[Type == ""http://contoso.com/email""] " +
                @"=> issue(claim = c1);";

            //Editors and email.
            var positive = new List<Claim>()
            {
                new Claim("http://contoso.com/role","editors"),
                new Claim("http://contoso.com/email", "editor1@contoso.com")
            };

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://contoso.com/role", "editors"));
            Assert.AreEqual(1, pass.Count(), "More than one claim was issued.");

            //editors and no email.
            var negative = new List<Claim>()
            {
                new Claim("http://contoso.com/role","editors")
            };

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://contoso.com/role", "editors"));
            Assert.AreEqual(0, fail.Count(), "At least one claim was issued.");
        }

        /// <summary>
        /// User has role of editors and an email.
        /// </summary>
        [Test]
        public void Scenario024()
        {
            const string EXPRESSION =
                @"c1:[Type == ""http://contoso.com/role"", Value==""Editors""] && " +
                @"c2:[Type == ""http://contoso.com/email""] " +
                @"=> issue(type = c1.type, value = RegExReplace(c1.value, ""Editor(?:s)?"", ""Managers""));";

            //Editors and email.
            var positive = new List<Claim>()
            {
                new Claim("http://contoso.com/role","editors"),
                new Claim("http://contoso.com/email", "editor1@contoso.com")
            };

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://contoso.com/role", "Managers"));
            Assert.AreEqual(1, pass.Count(), "More than one claim was issued.");

            //editors and no email.
            var negative = new List<Claim>()
            {
                new Claim("http://contoso.com/role","editors")
            };

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.IsFalse(this.Utility.HasIssuedClaim(fail, "http://contoso.com/role", "Managers"));
            Assert.AreEqual(0, fail.Count(), "At least one claim was issued.");
        }

        [Test]
        public void Scenario025()
        {
            const string EXPRESSION =
                @"c1:[Type == ""http://contoso.com/name"", Value=~""^Aidan""] " +
                @"=> issue(type = c1.type, value = RegExReplace(c1.value, ""Aidan"", ""Apple""));";

            //Editors and email.
            var positive = new List<Claim>()
            {
                new Claim("http://contoso.com/name","Aidan Gilbert")
            };

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://contoso.com/name", "Apple Gilbert"));
            Assert.AreEqual(1, pass.Count(), "More than one claim was issued.");

            //editors and no email.
            var negative = new List<Claim>()
            {
                new Claim("http://contoso.com/name","Steve")
            };

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.AreEqual(0, fail.Count(), "At least one claim was issued.");
        }

        [Test]
        public void Scenario027()
        {
            const string EXPRESSION =
                @"c1:[Type == ""http://contoso.com/name"", Value=~""^Aidan""] " +
                @"=> issue(type = c1.type, value = RegExReplace(c1.value, ""(?<first>[^\s]+)\s+(?<last>[^\s]+)"", ""${last} ${first}""));";

            //Editors and email.
            var positive = new List<Claim>()
            {
                new Claim("http://contoso.com/name","Aidan Gilbert")
            };

            var pass = this.Engine.Transform(EXPRESSION, positive);

            Assert.IsTrue(this.Utility.HasIssuedClaim(pass, "http://contoso.com/name", "Gilbert Aidan"));
            Assert.AreEqual(1, pass.Count(), "More than one claim was issued.");

            //editors and no email.
            var negative = new List<Claim>()
            {
                new Claim("http://contoso.com/name","Steve")
            };

            var fail = this.Engine.Transform(EXPRESSION, negative);

            Assert.AreEqual(0, fail.Count(), "At least one claim was issued.");
        }
    }
}
