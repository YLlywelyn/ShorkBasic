namespace ShorkBasic
{
    /// <summary>
    /// A token generated from the input text by the <see cref="Lexer"/>
    /// </summary>
    internal class Token
    {
        /// <summary>
        /// The type of the token.
        /// </summary>
        public TokenType type { get; protected set; }

        /// <summary>
        /// The tokens value.  For most token types this will be null.
        /// </summary>
        public dynamic value { get; protected set; }

        /// <summary>
        /// The position that marks the first character of this token.
        /// </summary>
        public Position startPosition { get; protected set; }

        /// <summary>
        /// The position that marks the last character of this token.
        /// </summary>
        public Position endPosition { get; protected set; }

        /// <summary>
        /// Create a new single-character token of the given type with no value.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="startPosition">The position marking the first character of the token.</param>
        public Token(TokenType type, Position startPosition)
            : this(type, startPosition, null, null) { }
        /// <summary>
        /// Create a new token of the given type with no value.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="startPosition">The position marking the first character of the token.</param>
        /// <param name="endPosition">The position marking the first character of the token.</param>
        public Token(TokenType type, Position startPosition, Position endPosition = null)
            : this(type, startPosition, endPosition, null) { }
        /// <summary>
        /// Create a new token of the given type.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="startPosition">The position marking the first character of the token.</param>
        /// <param name="endPosition">The position marking the first character of the token.</param>
        /// <param name="value">The value of the token.</param>
        public Token(TokenType type, Position startPosition, Position endPosition = null, dynamic value = null)
        {
            this.type = type;
            this.value = value;
            this.startPosition = startPosition.Copy();
            this.endPosition = (endPosition != null) ? endPosition.Copy() : startPosition.Copy();
        }

        public override string ToString()
        {
            if ((object)value == null) return string.Format("{{{0}}}", type);
            else return string.Format("{{{0}: {1}}}", type, value);
        }

        public bool Matches(TokenType type)
        {
            return this.type == type;
        }
        public bool Matches(TokenType type, dynamic value)
        {
            return (this.type == type) && (this.value == value);
        }
    }
}
