using System;
using System.Collections.Generic;
using System.Text;

namespace ClaimsTransformation.Language.Parser
{
    public abstract class ParserBase
    {
        public virtual bool TryParse(StringReader reader, Syntax syntax, out TokenValue result)
        {
            if (syntax.Flags.HasFlag(SyntaxFlags.Repeat))
            {
                return this.TryParseMultiple(reader, syntax, out result);
            }
            else
            {
                return this.TryParseSingle(reader, syntax, out result);
            }
        }

        protected virtual bool TryParseSingle(StringReader reader, Syntax syntax, out TokenValue result)
        {
            if (syntax.Token != null)
            {
                return this.TryParse(reader, syntax, syntax.Token, out result);
            }
            else if (syntax.Children != null)
            {
                if (syntax.Flags.HasFlag(SyntaxFlags.Any))
                {
                    return this.TryParseAny(reader, syntax, syntax.Children, out result);
                }
                if (syntax.Flags.HasFlag(SyntaxFlags.All))
                {
                    return this.TryParseAll(reader, syntax, syntax.Children, out result);
                }
            }
            throw new NotImplementedException();
        }

        protected virtual bool TryParseMultiple(StringReader reader, Syntax syntax, out TokenValue result)
        {
            var values = new List<TokenValue>();
            while (this.TryParseSingle(reader, syntax, out result))
            {
                values.Add(result);
            }
            switch (values.Count)
            {
                case 0:
                    result = default(TokenValue);
                    return false;
                case 1:
                    result = values[0];
                    return true;
                default:
                    result = new TokenValue(syntax, values);
                    return true;
            }
        }

        protected virtual bool TryParseAny(StringReader reader, Syntax syntax, IEnumerable<Syntax> children, out TokenValue result)
        {
            foreach (var child in children)
            {
                if (this.TryParse(reader, child, out result))
                {
                    return true;
                }
            }
            result = default(TokenValue);
            return false;
        }

        protected virtual bool TryParseAll(StringReader reader, Syntax syntax, IEnumerable<Syntax> children, out TokenValue result)
        {
            reader.Begin();
            reader.Align();
            var values = new List<TokenValue>();
            foreach (var child in children)
            {
                if (this.TryParse(reader, child, out result))
                {
                    values.Add(result);
                }
                else
                {
                    reader.Rollback();
                    result = default(TokenValue);
                    return false;
                }
            }
            reader.Complete();
            result = new TokenValue(syntax, values);
            return true;
        }

        protected virtual bool TryParse(StringReader reader, Syntax syntax, Token token, out TokenValue result)
        {
            if (token.IsEmpty)
            {
                result = new TokenValue(syntax, token, string.Empty);
                return true;
            }
            if (token.Flags.HasFlag(TokenFlags.Literal))
            {
                return this.TryParseLiteral(reader, syntax, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.Identifier))
            {
                return this.TryParseIdentifier(reader, syntax, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.Boolean))
            {
                return this.TryParseBoolean(reader, syntax, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.Number))
            {
                return this.TryParseNumber(reader, syntax, token, out result);
            }
            if (token.Flags.HasFlag(TokenFlags.String))
            {
                return this.TryParseString(reader, syntax, token, out result);
            }
            throw new NotImplementedException();
        }

        protected virtual bool TryParseLiteral(StringReader reader, Syntax syntax, Token token, out TokenValue result)
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
                    result = new TokenValue(syntax, token, token.Value);
                    return true;
                }
            }
            reader.Rollback();
            result = default(TokenValue);
            return false;
        }

        protected virtual bool TryParseIdentifier(StringReader reader, Syntax syntax, Token token, out TokenValue result)
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
                result = new TokenValue(syntax, token, builder.ToString());
                return true;
            }
            reader.Rollback();
            result = default(TokenValue);
            return false;
        }

        protected virtual bool TryParseBoolean(StringReader reader, Syntax syntax, Token token, out TokenValue result)
        {
            //TODO: Implement me.
            result = default(TokenValue);
            return false;
        }

        protected virtual bool TryParseNumber(StringReader reader, Syntax syntax, Token token, out TokenValue result)
        {
            reader.Begin();
            reader.Align();
            if (char.IsNumber(reader.Peek()))
            {
                var builder = new StringBuilder();
                do
                {
                    builder.Append(reader.Read());
                } while (char.IsNumber(reader.Peek()));
                reader.Complete();
                result = new TokenValue(syntax, token, builder.ToString());
                return true;
            }
            reader.Rollback();
            result = default(TokenValue);
            return false;
        }

        protected virtual bool TryParseString(StringReader reader, Syntax syntax, Token token, out TokenValue result)
        {
            reader.Begin();
            reader.Align();
            if (reader.Read() == '"')
            {
                var builder = new StringBuilder();
                while (!reader.EOF)
                {
                    var character = reader.Read();
                    if (character == '"')
                    {
                        reader.Complete();
                        result = new TokenValue(syntax, token, builder.ToString());
                        return true;
                    }
                    builder.Append(character);
                }
            }
            reader.Rollback();
            result = default(TokenValue);
            return false;
        }
    }
}
