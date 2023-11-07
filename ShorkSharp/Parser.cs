namespace ShorkSharp
{
    public class Parser
    {
        Token[] tokens;
        int tokenIndex = 0;
        Token currentToken;

        public Parser(Token[] tokens)
        {
            this.tokens = tokens;
            this.currentToken = this.tokens[0];
        }

        public void Advance()
        {
            tokenIndex++;
            currentToken = (tokenIndex < tokens.Length) ? this.tokens[tokenIndex] : null;
        }

        public ParseResult Parse()
        {
            ParseResult result = ParseExpression();

            if (result.error != null && currentToken.type != TokenType.EOF)
                return result.Failure(new InvalidSyntaxError("Unexpected EOF", currentToken.startPosition));

            return result;
        }

        //##################################

        protected ParseResult ParseExpression()
        {
            throw new NotImplementedException();
        }
    }
}
