namespace ShorkSharp
{
    internal class Token
    {
        public TokenType type { get; protected set; }
        public dynamic value { get; protected set; }

        public Position startPosition { get; protected set; }
        public Position endPosition { get; protected set; }

        internal Token(TokenType type, Position startPosition)
            : this(type, null, startPosition, startPosition) { }
        internal Token(TokenType type, dynamic value, Position startPosition, Position endPosition = null)
        {
            this.type = type;
            this.value = value;
            this.startPosition = startPosition.Copy();
            this.endPosition = endPosition == null ? startPosition.Copy() : endPosition.Copy();
        }

        public override string ToString()
        {
            if (value == null)
                return string.Format("[{0}]", type);
            else
                return string.Format("[{0} : {1}]", type, value);
        }
    }
}
