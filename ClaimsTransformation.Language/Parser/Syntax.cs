using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaimsTransformation.Language.Parser
{
    public class Syntax
    {
        public Syntax(Token token) : this(token, Enumerable.Empty<Syntax>(), SyntaxFlags.None)
        {

        }

        public Syntax(IEnumerable<Syntax> children, SyntaxFlags flags) : this(null, children, flags)
        {

        }

        public Syntax(Token token, IEnumerable<Syntax> children, SyntaxFlags flags)
        {
            this.Token = token;
            this.Children = children;
            this.Flags = flags;
        }

        public Token Token { get; private set; }

        public IEnumerable<Syntax> Children { get; private set; }

        public SyntaxFlags Flags { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Token != null)
                {
                    hashCode += this.Token.GetHashCode();
                }
                if (this.Children != null)
                {
                    foreach (var child in this.Children)
                    {
                        hashCode += child.GetHashCode();
                    }
                }
                hashCode += this.Flags.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (this.Token != null)
            {
                return this.Token.ToString();
            }
            else if (this.Children != null)
            {
                var builder = new StringBuilder();
                foreach (var child in this.Children)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(child.ToString());
                }
                return builder.ToString();
            }
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Syntax);
        }

        public virtual bool Equals(Syntax other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Token, other.Token))
            {
                return false;
            }
            if (!Enumerable.SequenceEqual(this.Children, other.Children))
            {
                return false;
            }
            if (this.Flags != other.Flags)
            {
                return false;
            }
            return true;
        }

        public static bool operator ==(Syntax a, Syntax b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }
            if ((object)a == null || (object)b == null)
            {
                return false;
            }
            if (object.ReferenceEquals((object)a, (object)b))
            {
                return true;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Syntax a, Syntax b)
        {
            return !(a == b);
        }
    }

    [Flags]
    public enum SyntaxFlags
    {
        None = 0,
        Any = 1,
        All = 2,
        Repeat = 4
    }
}
