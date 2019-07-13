using System.Collections.Generic;

namespace ClaimsTransformation.Language.Parser
{
    public class StringReader
    {
        private StringReader()
        {
            this.Markers = new Stack<int>();
        }

        public StringReader(string value) : this()
        {
            this.Value = value;
        }

        public Stack<int> Markers { get; private set; }

        public string Value { get; private set; }

        public int Position { get; private set; }

        public bool EOF
        {
            get
            {
                return this.Position >= this.Value.Length;
            }
        }

        public char Read()
        {
            var value = default(char);
            if (this.Position < this.Value.Length)
            {
                value = this.Value[this.Position];
                this.Position++;
            }
            return value;
        }

        public char Peek()
        {
            var value = default(char);
            if (this.Position < this.Value.Length)
            {
                value = this.Value[this.Position];
            }
            return value;
        }

        public void Align()
        {
            while (!this.EOF)
            {
                var character = this.Peek();
                if (!char.IsWhiteSpace(character))
                {
                    break;
                }
                this.Position++;
            }
        }

        public void Begin()
        {
            this.Markers.Push(this.Position);
        }

        public void Complete()
        {
            this.Markers.Pop();
        }

        public void Rollback()
        {
            this.Position = this.Markers.Pop();
        }
    }
}
