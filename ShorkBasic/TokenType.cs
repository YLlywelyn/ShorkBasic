namespace ShorkBasic
{
    /// <summary>
    /// A value to determine the type of a given token.
    /// </summary>
    [Flags]
    internal enum TokenType
    {
        EOF         = 1,

        INT         = 1 << 1,
        FLOAT       = 1 << 2,
        STRING      = 1 << 3,
        
        IDENTIFIER  = 1 << 4,
        KEYWORD     = 1 << 5,

        PLUS        = 1 << 6,
        MINUS       = 1 << 7,
        MULTIPLY    = 1 << 8,
        DIVIDE      = 1 << 9,
        EXPONENT    = 1 << 10,

        LPAREN      = 1 << 11,
        RPAREN      = 1 << 12,
        LBRACE      = 1 << 13,
        RBRACE      = 1 << 14,
        LBRACKET    = 1 << 15,
        RBRACKET    = 1 << 16,
    }
}
