using ClaimsTransformation.Language.DOM;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClaimsTransformation.Language.Parser
{
    public class ClaimsTransformationParser : IClaimsTransformationParser
    {
        public ClaimsTransformationParser()
        {

        }

        public IEnumerable<RuleExpression> Parse(IEnumerable<string> expressions)
        {
            foreach (var expression in expressions)
            {
                yield return this.Parse(expression);
            }
        }

        protected virtual RuleExpression Parse(string expression)
        {
            var reader = new StringReader(expression);
            return this.Parse(reader);
        }

        protected virtual RuleExpression Parse(StringReader reader)
        {
            var result = default(TokenValue);
            var success = this.TryParse(reader, ClaimsTransformationSyntax.Syntax, out result);


            throw new NotImplementedException();
        }

        protected virtual bool TryParse(StringReader reader, Syntax syntax, out TokenValue result)
        {
            if (syntax.Token != null)
            {
                return this.TryParse(reader, syntax.Token, out result);
            }
            else if (syntax.Children != null)
            {
                if (syntax.Flags.HasFlag(SyntaxFlags.Any))
                {
                    return this.TryParseAny(reader, syntax.Children, out result);
                }
                if (syntax.Flags.HasFlag(SyntaxFlags.All))
                {
                    return this.TryParseAll(reader, syntax.Children, out result);
                }
            }
            throw new NotImplementedException();
        }

        protected virtual bool TryParseAny(StringReader reader, IEnumerable<Syntax> children, out TokenValue result)
        {
            foreach (var syntax in children)
            {
                if (this.TryParse(reader, syntax, out result))
                {
                    return true;
                }
            }
            result = default(TokenValue);
            return false;
        }

        protected virtual bool TryParseAll(StringReader reader, IEnumerable<Syntax> children, out TokenValue result)
        {
            var values = new List<TokenValue>();
            foreach (var syntax in children)
            {
                if (this.TryParse(reader, syntax, out result))
                {
                    values.Add(result);
                }
                else
                {
                    result = default(TokenValue);
                    return false;
                }
            }
            result = new TokenValue(values);
            return true;
        }

        protected virtual bool TryParse(StringReader reader, Token token, out TokenValue result)
        {
            if (token.Flags.HasFlag(TokenFlags.Literal))
            {
                return this.TryParseLiteral(reader, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.Identifier))
            {
                return this.TryParseIdentifier(reader, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.Boolean))
            {
                return this.TryParseBoolean(reader, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.Number))
            {
                return this.TryParseNumber(reader, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.String))
            {
                return this.TryParseString(reader, token, out result);
            }
            throw new NotImplementedException();
        }

        protected virtual bool TryParseLiteral(StringReader reader, Token token, out TokenValue result)
        {
            reader.Begin();
            reader.Align();
            var builder = new StringBuilder();
            while (!reader.EOF)
            {
                builder.Append(reader.Read());
                if (string.Equals(builder.ToString(), token.Value, StringComparison.OrdinalIgnoreCase))
                {
                    reader.Complete();
                    result = new TokenValue(token, token.Value);
                    return true;
                }
            }
            reader.Rollback();
            result = default(TokenValue);
            return false;
        }

        private bool TryParseIdentifier(StringReader reader, Token token, out TokenValue result)
        {
            reader.Begin();
            reader.Align();
            var builder = new StringBuilder();
            while (!reader.EOF)
            {
                if (!char.IsLetter(reader.Peek()))
                {
                    break;
                }
                builder.Append(reader.Read());
                while (!reader.EOF)
                {
                    if (!char.IsLetterOrDigit(reader.Peek()))
                    {
                        break;
                    }
                    builder.Append(reader.Read());
                }
                reader.Complete();
                result = new TokenValue(token, builder.ToString());
                return true;
            }
            reader.Rollback();
            result = default(TokenValue);
            return false;
        }

        private bool TryParseBoolean(StringReader reader, Token token, out TokenValue result)
        {
            throw new NotImplementedException();
        }

        private bool TryParseNumber(StringReader reader, Token token, out TokenValue result)
        {
            throw new NotImplementedException();
        }

        private bool TryParseString(StringReader reader, Token token, out TokenValue result)
        {
            throw new NotImplementedException();
        }
    }
}
