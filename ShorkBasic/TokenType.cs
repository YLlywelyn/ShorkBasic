namespace ShorkBasic
{
    [Flags]
    internal enum TokenType
    {
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
