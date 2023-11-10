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

        Token Advance()
        {
            tokenIndex++;
            currentToken = (tokenIndex < tokens.Length) ? this.tokens[tokenIndex] : null;
            return currentToken;
        }

        Token Reverse(int amount = 1)
        {
            tokenIndex -= amount;
            currentToken = (tokenIndex < tokens.Length) ? this.tokens[tokenIndex] : null;
            return currentToken;
        }

        public ParseResult Parse()
        {
            ParseResult result = ParseStatements();

            if (result.error != null && currentToken.type != TokenType.EOF)
                return result.Failure(new InvalidSyntaxError("Unexpected EOF", currentToken.startPosition));

            return result;
        }

        //##################################

        protected ParseResult ParseStatements()
        {
            ParseResult result = new ParseResult();
            List<NodeBase> statements = new List<NodeBase>();
            Position startPosition = currentToken.startPosition.Copy();

            while (currentToken.type != TokenType.NEWLINE)
            {
                result.RegisterAdvancement();
                Advance();
            }

            NodeBase statement = result.Register(ParseStatement());
            if (result.error != null)
                return result;
            statements.Add(statement);

            bool hasMoreStatements = true;
            while (true)
            {
                int newlineCount = 0;
                while (currentToken.type == TokenType.NEWLINE)
                {
                    result.RegisterAdvancement();
                    Advance();
                    newlineCount++;
                }
                if (newlineCount == 0)
                    hasMoreStatements = false;

                if (!hasMoreStatements)
                    break;

                statement = result.TryRegister(ParseStatement());
                if (statement == null)
                {
                    Reverse(result.toReverseCount);
                    hasMoreStatements = false;
                    continue;
                }
                statements.Add(statement);
            }

            return result.Success(new CodeBlockNode(statements, startPosition, currentToken.endPosition));
        }

        protected ParseResult ParseStatement()
        {
            ParseResult result = new ParseResult();
            Position startPosition = currentToken.startPosition.Copy();

            if (currentToken.Matches(TokenType.KEYWORD, "return"))
            {
                result.RegisterAdvancement();
                Advance();

                NodeBase expression = result.TryRegister(ParseExpression());
                if (expression == null)
                {
                    Reverse(result.toReverseCount);
                    return result.Success(new ReturnNode(startPosition, currentToken.endPosition));
                }
                else
                    return result.Success(new ReturnNode(expression));
            }

            else if (currentToken.Matches(TokenType.KEYWORD, "continue"))
            {
                result.RegisterAdvancement();
                Advance();
                return result.Success(new ContinueNode(startPosition, currentToken.endPosition));
            }

            else if (currentToken.Matches(TokenType.KEYWORD, "break"))
            {
                result.RegisterAdvancement();
                Advance();
                return result.Success(new BreakNode(startPosition, currentToken.endPosition));
            }

            else
            {
                NodeBase expression = result.Register(ParseExpression());
                if (result.error != null)
                    return result.Failure(new InvalidSyntaxError("Expected 'RETURN', 'CONTINUE', 'BREAK', 'VAR', 'IF', 'FOR', 'WHILE', 'FUN', int, float, identifier, '+', '-', '(', '[' or 'NOT'", currentToken.startPosition));

                return result.Success(expression);
            }
        }

        protected ParseResult ParseExpression()
        {
            ParseResult result = new ParseResult();

            if (currentToken.Matches(TokenType.KEYWORD, "var"))
            {
                result.RegisterAdvancement();
                Advance();

                if (currentToken.type != TokenType.IDENTIFIER)
                    return result.Failure(new InvalidSyntaxError("Expected identifier", currentToken.startPosition));

                Token varNameToken = currentToken;
                result.RegisterAdvancement();
                Advance();

                if (currentToken.type != TokenType.EQUALS)
                    return result.Failure(new InvalidSyntaxError("Expected '='", currentToken.startPosition));

                result.RegisterAdvancement();
                Advance();

                NodeBase expression = result.Register(ParseExpression());
                if (result.error != null) return result;
                return result.Success(new VarAssignNode(varNameToken, expression));
            }

            else
            {
                NodeBase node = result.Register(ParseBinaryOperation(ParseComparisonExpression, new (TokenType, string)[] { (TokenType.KEYWORD, "and"), (TokenType.KEYWORD, "or") }));
                if (result.error != null)
                    return result.Failure(new InvalidSyntaxError("Expected 'VAR', 'IF', 'FOR', 'WHILE', 'FUNC', number, identifier, '+', '-', '(', '[' or 'NOT'", currentToken.startPosition));
                return result.Success(node);
            }
        }

        protected ParseResult ParseComparisonExpression()
        {
            ParseResult result = new ParseResult();
            NodeBase node;

            if (currentToken.Matches(TokenType.KEYWORD, "not"))
            {
                Token operatorToken = currentToken;
                result.RegisterAdvancement();
                Advance();

                node = result.Register(ParseComparisonExpression());
                if (result.error != null) return result;
                return result.Success(node);
            }

            node = result.Register(ParseBinaryOperation(ParseArithmaticExpression, new TokenType[] { TokenType.DOUBLE_EQUALS, TokenType.NOT_EQUALS, TokenType.LESS_THAN, TokenType.GREATER_THAN, TokenType.LESS_THAN_OR_EQUAL, TokenType.GREATER_THAN_OR_EQUAL }));
            if (result.error != null)
                return result.Failure(new InvalidSyntaxError("Expected number, identifier, '+', '-', '(', '[', 'IF', 'FOR', 'WHILE', 'FUNC' or 'NOT'", currentToken.startPosition));
            return result.Success(node);
        }

        protected ParseResult ParseArithmaticExpression()
        {
            return ParseBinaryOperation(ParseTerm, new TokenType[] { TokenType.PLUS, TokenType.MINUS });
        }

        protected ParseResult ParseTerm()
        {
            return ParseBinaryOperation(ParseFactor, new TokenType[] { TokenType.MULTIPLY, TokenType.DIVIDE });
        }

        protected ParseResult ParseFactor()
        {
            throw new NotImplementedException();
        }

        protected ParseResult ParseExponent()
        {
            throw new NotImplementedException();
        }

        protected ParseResult ParseCall()
        {
            throw new NotImplementedException();
        }

        protected ParseResult ParseAtom()
        {
            throw new NotImplementedException();
        }

        protected ParseResult ParseListExpression()
        {
            throw new NotImplementedException();
        }

        /* TODO: ParseIfExpression

        protected ParseResult ParseStatement()
        {
            throw new NotImplementedException();
        }*/

        protected ParseResult ParseForExpression()
        {
            throw new NotImplementedException();
        }

        protected ParseResult ParseWhileExpression()
        {
            throw new NotImplementedException();
        }

        protected ParseResult ParseFunctionDefinition()
        {
            throw new NotImplementedException();
        }

        //######################################

        protected delegate ParseResult BinaryOperationDelegate();
        protected ParseResult ParseBinaryOperation(BinaryOperationDelegate leftFunc, TokenType[] operations)
        {
            return ParseBinaryOperation(leftFunc, operations, leftFunc);
        }
        protected ParseResult ParseBinaryOperation(BinaryOperationDelegate leftFunc, TokenType[] operations, BinaryOperationDelegate rightFunc)
        {
            ParseResult result = new ParseResult();

            NodeBase leftNode = result.Register(leftFunc());
            if (result.error != null)
                return result;

            while (operations.Contains(currentToken.type))
            {
                Token operatorToken = currentToken;
                result.RegisterAdvancement();
                Advance();

                NodeBase rightNode = result.Register(rightFunc());
                if (result.error != null)
                    return result;

                leftNode = new BinaryOperationNode(leftNode, operatorToken, rightNode);
            }

            return result.Success(leftNode);
        }
        protected ParseResult ParseBinaryOperation(BinaryOperationDelegate leftFunc, (TokenType, string)[] operations)
        {
            return ParseBinaryOperation(leftFunc, operations, leftFunc);
        }
        protected ParseResult ParseBinaryOperation(BinaryOperationDelegate leftFunc, (TokenType, string)[] operations, BinaryOperationDelegate rightFunc)
        {
            ParseResult result = new ParseResult();

            NodeBase leftNode = result.Register(leftFunc());
            if (result.error != null)
                return result;

            while (operations.Contains((currentToken.type, (string)currentToken.value)))
            {
                Token operatorToken = currentToken;
                result.RegisterAdvancement();
                Advance();

                NodeBase rightNode = result.Register(rightFunc());
                if (result.error != null)
                    return result;

                leftNode = new BinaryOperationNode(leftNode, operatorToken, rightNode);
            }

            return result.Success(leftNode);
        }
    }
}
