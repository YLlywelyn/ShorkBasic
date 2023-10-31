namespace ShorkBasic
{
    /// <summary>
    /// A value to determine the type of a given token.
    /// </summary>
    [Flags]
    internal enum TokenType
    {
        EOF,

        INT,
        FLOAT,
        STRING,
        
        IDENTIFIER,
        KEYWORD,

        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        EXPONENT,

        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,
        LBRACKET,
        RBRACKET,
    }
}
