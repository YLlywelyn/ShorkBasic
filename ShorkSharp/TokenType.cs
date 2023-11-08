namespace ShorkSharp
{
    public enum TokenType
    {
        NUMBER,
        STRING,
        BOOL,
        NULL,

        KEYWORD,
        IDENTIFIER,

        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        EXPONENT,

        EQUALS,
        DOUBLE_EQUALS,
        NOT_EQUALS,
        LESS_THAN,
        GREATER_THAN,
        LESS_THAN_OR_EQUAL,
        GREATER_THAN_OR_EQUAL,
        
        DOT,
        COMMA,
        ARROW,

        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,
        LBRACKET,
        RBRACKET,

        NEWLINE,
        EOF
    }
}
