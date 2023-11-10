namespace ShorkSharp
{
    public class Token
    {
        public TokenType type { get; protected set; }
        public dynamic value { get; protected set; }

        public Position startPosition { get; protected set; }
        public Position endPosition { get; protected set; }

        public Token(TokenType type, Position startPosition)
        {
            this.type = type;
            this.value = null;
            this.startPosition = startPosition.Copy();
            this.endPosition = startPosition.Copy();
        }
        public Token(TokenType type, Position startPosition, Position endPosition)
        {
            this.type = type;
            this.value = null;
            this.startPosition = startPosition.Copy();
            this.endPosition = endPosition.Copy();
        }
        public Token(TokenType type, dynamic value, Position startPosition)
        {
            this.type = type;
            this.value = value;
            this.startPosition = startPosition.Copy();
            this.endPosition = startPosition.Copy();
        }
        public Token(TokenType type, dynamic value, Position startPosition, Position endPosition)
        {
            this.type = type;
            this.value = value;
            this.startPosition = startPosition.Copy();
            this.endPosition = endPosition.Copy();
        }

        public bool Matches(TokenType type)
        {
            return this.type == type;
        }
        public bool Matches(TokenType type, dynamic value)
        {
            if (type == TokenType.KEYWORD)
                return this.type == type && ((string)this.value).ToLower() == ((string)value).ToLower();
            return this.type == type && this.value == value;
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
