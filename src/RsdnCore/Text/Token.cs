namespace Rsdn.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Token : IEquatable<string>, IEquatable<char>
    {
        private readonly string source;

        internal Token(string source, int index, int length)
        {
            this.source = source;
            this.Index = index;
            this.Length = length;
        }

        public int Index { get; }
        public int Length { get; }
        public abstract TokenCategory Category { get; }

        public bool HasLeadingSpace
        {
            get
            {
                return Index > 0 && Whitespace.TestCharacter(this.source[Index - 1]);
            }
        }

        public bool HasTrailingSpace
        {
            get
            {
                var idx = Index + Length;
                return idx < this.source.Length && Whitespace.TestCharacter(this.source[idx]);
            }
        }

        internal string Source => this.source;

        public static implicit operator string(Token token)
        {
            if (Object.ReferenceEquals(token, null) == false)
                return token.ToString();
            return null;
        }

        public static bool operator ==(Token a, Token b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;

            if (Object.ReferenceEquals(a, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator ==(Token a, string b)
        {
            if (Object.ReferenceEquals(a, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Token a, Token b)
        {
            return !(a == b);
        }

        public static bool operator !=(Token a, string b)
        {
            return !(a == b);
        }

        public sealed override string ToString() => this.source.Substring(this.Index, this.Length);

        public sealed override int GetHashCode()
        {
            unchecked
            {
                var result = 17;

                result = result * 23 + this.source.GetHashCode();
                result = result * 23 + this.Index.GetHashCode();
                result = result * 23 + this.Length.GetHashCode();

                return result;
            }
        }

        public sealed override bool Equals(object obj)
        {
            var token = obj as Token;
            if (Object.ReferenceEquals(token, null) == false)
                return Equals(token.source, token.Index, token.Length);

            return false;
        }

        public bool Equals(string other)
        {
            return Equals(other, 0, other.Length);
        }

        public bool Equals(char other)
        {
            if (this.Length != 1)
                return false;

            return this.source[this.Index].Equals(other);
        }

        private bool Equals(string other, int otherIndex, int otherLength)
        {
            if (this.Length != other.Length)
                return false;

            return String.Compare(this.source, this.Index,
                other, otherIndex, otherLength,
                StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}