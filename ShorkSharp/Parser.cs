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
            ParseResult result = new ParseResult();

            if (currentToken.Matches(TokenType.PLUS, TokenType.MINUS))
            {
                Token operandToken = currentToken;
                result.RegisterAdvancement();
                Advance();
                NodeBase factor = result.Register(ParseFactor());
                if (result.error != null) return result;
                return result.Success(new UnaryOperationNode(operandToken, factor));
            }

            return ParseExponent();
        }

        protected ParseResult ParseExponent()
        {
            return ParseBinaryOperation(ParseCall, new TokenType[] { TokenType.EXPONENT }, ParseFactor);
        }

        protected ParseResult ParseCall()
        {
            ParseResult result = new ParseResult();

             NodeBase atom = result.Register(ParseAtom());
            if (result.error != null) return result;

            if (currentToken.type == TokenType.LPAREN)
            {
                result.RegisterAdvancement();
                Advance();

                List<NodeBase> args = new List<NodeBase>();

                if (currentToken.type == TokenType.RPAREN)
                {
                    result.RegisterAdvancement();
                    Advance();
                }
                else
                {
                    args.Add(result.Register(ParseExpression()));
                    if (result.error != null)
                        return result.Failure(new InvalidSyntaxError("Expected ')', 'VAR', 'IF', 'FOR', 'WHILE', 'FUNC', number, identifier, '+', '-', '(', '[' or 'NOT'", currentToken.startPosition));

                    while (currentToken.type == TokenType.COMMA)
                    {
                        result.RegisterAdvancement();
                        Advance();

                        args.Add(result.Register(ParseExpression()));
                        if (result.error != null) return result;
                    }

                    if (currentToken.type != TokenType.RPAREN)
                        return result.Failure(new InvalidSyntaxError("Expected ',' or ')'", currentToken.startPosition));

                    result.RegisterAdvancement();
                    Advance();
                }

                return result.Success(new CallNode(atom, args.ToArray()));
            }
            return result.Success(atom);
        }

        protected ParseResult ParseAtom()
        {
            ParseResult result = new ParseResult();

            if (currentToken.type == TokenType.NUMBER)
            {
                result.RegisterAdvancement();
                Advance();
                return result.Success(new NumberNode(currentToken));
            }

            else if (currentToken.type == TokenType.STRING)
            {
                result.RegisterAdvancement();
                Advance();
                return result.Success(new StringNode(currentToken));
            }

            else if (currentToken.type == TokenType.IDENTIFIER)
            {
                result.RegisterAdvancement();
                Advance();
                return result.Success(new VarAccessNode(currentToken));
            }

            else if (currentToken.type == TokenType.LPAREN)
            {
                result.RegisterAdvancement();
                Advance();

                NodeBase expression = result.Register(ParseExpression());
                if (result.error != null) return result;

                if (currentToken.type == TokenType.RPAREN)
                {
                    result.RegisterAdvancement();
                    Advance();
                    return result.Success(expression);
                }
                else return result.Failure(new InvalidSyntaxError("Expected ')'", currentToken.startPosition));
            }

            else if (currentToken.type == TokenType.LBRACKET)
            {
                NodeBase list = result.Register(ParseListExpression());
                if (result.error != null) return result;
                return result.Success(list);
            }

            else if (currentToken.Matches(TokenType.KEYWORD, "if"))
            {
                NodeBase ifNode = result.Register(ParseIfExpression());
                if (result.error != null) return result;
                return result.Success(ifNode);
            }

            else if (currentToken.Matches(TokenType.KEYWORD, "for"))
            {
                NodeBase forNode = result.Register(ParseForExpression());
                if (result.error != null) return result;
                return result.Success(forNode);
            }

            else if (currentToken.Matches(TokenType.KEYWORD, "while"))
            {
                NodeBase whileNode = result.Register(ParseWhileExpression());
                if (result.error != null) return result;
                return result.Success(whileNode);
            }

            else if (currentToken.Matches(TokenType.KEYWORD, "func"))
            {
                NodeBase functionDefinition = result.Register(ParseFunctionDefinition());
                if (result.error != null) return result;
                return result.Success(functionDefinition);
            }

            else return result.Failure(new InvalidSyntaxError("Expected number, identifier, '+', '-', '(', '[', IF', 'FOR', 'WHILE', 'FUNC'", currentToken.startPosition));
        }

        protected ParseResult ParseListExpression()
        {
            ParseResult result = new ParseResult();

            List<NodeBase> elements = new List<NodeBase>();
            Position startPosition = currentToken.startPosition.Copy();

            if (currentToken.type != TokenType.LBRACKET)
                return result.Failure(new InvalidSyntaxError("Expected '['", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            if (currentToken.type == TokenType.RBRACKET)
            {
                result.RegisterAdvancement();
                Advance();
            }
            else
            {
                elements.Add(result.Register(ParseExpression()));
                if (result.error != null)
                    return result.Failure(new InvalidSyntaxError("Expected ']', 'VAR', 'IF', 'FOR', 'WHILE', 'FUNC', number, identifier, '+', '-', '(', '[' or 'NOT'", currentToken.startPosition));

                while (currentToken.type == TokenType.COMMA)
                {
                    result.RegisterAdvancement();
                    Advance();

                    elements.Add(result.Register(ParseExpression()));
                    if (result.error != null) return result;
                }

                if (currentToken.type != TokenType.RBRACKET)
                    return result.Failure(new InvalidSyntaxError("Expected ']'", currentToken.startPosition));

                result.RegisterAdvancement();
                Advance();
            }

            return result.Success(new ListNode(elements, startPosition, currentToken.endPosition));
        }

        protected ParseResult ParseIfExpression()
        {
            ParseIfResult result = new ParseIfResult();

            List<(NodeBase condition, NodeBase body, bool shouldReturnNull)> cases = new List<(NodeBase condition, NodeBase body, bool shouldReturnNull)>();
        }
        protected ParseIfResult ParseIfCase(string ifKeyword)
        {
            ParseIfResult result = new ParseIfResult();

        }

        protected ParseResult ParseForExpression()
        {
            ParseResult result = new ParseResult();

            if (!currentToken.Matches(TokenType.KEYWORD, "for"))
                return result.Failure(new InvalidSyntaxError("Expected 'FOR'", currentToken.startPosition));

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

            NodeBase startValue = result.Register(ParseExpression());
            if (result.error != null) return result;

            if (!currentToken.Matches(TokenType.KEYWORD, "to"))
                return result.Failure(new InvalidSyntaxError("Expected 'TO'", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            NodeBase endValue = result.Register(ParseExpression());
            if (result.error != null) return result;

            NodeBase stepValue = null;
            if (currentToken.Matches(TokenType.KEYWORD, "step"))
            {
                result.RegisterAdvancement();
                Advance();

                stepValue = result.Register(ParseExpression());
                if (result.error != null) return result;
            }

            if (!currentToken.Matches(TokenType.KEYWORD, "do"))
                return result.Failure(new InvalidSyntaxError("Expected 'DO'", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            if (currentToken.type == TokenType.NEWLINE)
            {
                result.RegisterAdvancement();
                Advance();

                NodeBase bodyNodes = result.Register(ParseStatements());
                if (result.error != null) return result;

                if (!currentToken.Matches(TokenType.KEYWORD, "end"))
                    return result.Failure(new InvalidSyntaxError("Expected 'END'", currentToken.startPosition));

                result.RegisterAdvancement();
                Advance();

                return result.Success(new ForNode(varNameToken, startValue, endValue, stepValue, bodyNodes, true));
            }

            NodeBase bodyNode = result.Register(ParseStatement());
            if (result.error != null) return result;

            return result.Success(new ForNode(varNameToken, startValue, endValue, stepValue, bodyNode, false));
        }

        protected ParseResult ParseWhileExpression()
        {
            ParseResult result = new ParseResult();

            if (!currentToken.Matches(TokenType.KEYWORD, "while"))
                return result.Failure(new InvalidSyntaxError("Expected 'WHILE'", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            NodeBase condition = result.Register(ParseExpression());
            if (result.error != null) return result;

            if (!currentToken.Matches(TokenType.KEYWORD, "do"))
                return result.Failure(new InvalidSyntaxError("Expected 'do'", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            NodeBase bodyNode;
            if (currentToken.type == TokenType.NEWLINE)
            {
                result.RegisterAdvancement();
                Advance();

                bodyNode = result.Register(ParseStatements());
                if (result.error != null) return result;

                if (!currentToken.Matches(TokenType.KEYWORD, "end"))
                    return result.Failure(new InvalidSyntaxError("Expected 'END'", currentToken.startPosition));

                result.RegisterAdvancement();
                Advance();

                return result.Success(new WhileNode(condition, bodyNode, true));
            }
            else
            {
                bodyNode = result.Register(ParseStatement());
                if (result.error != null) return result;

                return result.Success(new WhileNode(condition, bodyNode, false));
            }
        }

        protected ParseResult ParseFunctionDefinition()
        {
            ParseResult result = new ParseResult();

            if (!currentToken.Matches(TokenType.KEYWORD, "func"))
                return result.Failure(new InvalidSyntaxError("Expected 'FUNC'", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            Token varNameToken = null;
            if (currentToken.type == TokenType.IDENTIFIER)
            {
                varNameToken = currentToken;

                result.RegisterAdvancement();
                Advance();

                if (currentToken.type != TokenType.LPAREN)
                    return result.Failure(new InvalidSyntaxError("Expected '('", currentToken.startPosition));
            }
            else
            {
                if (currentToken.type != TokenType.LPAREN)
                    return result.Failure(new InvalidSyntaxError("Expected identifier or '('", currentToken.startPosition));
            }

            result.RegisterAdvancement();
            Advance();

            List<Token> argTokens = new List<Token>();

            if (currentToken.type == TokenType.IDENTIFIER)
            {
                argTokens.Add(currentToken);
                result.RegisterAdvancement();
                Advance();

                while (currentToken.type == TokenType.COMMA)
                {
                    result.RegisterAdvancement();
                    Advance();

                    if (currentToken.type != TokenType.IDENTIFIER)
                        return result.Failure(new InvalidSyntaxError("Expected identifier", currentToken.startPosition));

                    argTokens.Add(currentToken);
                    result.RegisterAdvancement();
                    Advance();
                }

                if (currentToken.type != TokenType.RPAREN)
                    return result.Failure(new InvalidSyntaxError("Expected ',' or ')'", currentToken.startPosition));
            }
            else
            {
                if (currentToken.type != TokenType.RPAREN)
                    return result.Failure(new InvalidSyntaxError("Expected identifier or ')'", currentToken.startPosition));
            }

            result.RegisterAdvancement();
            Advance();

            NodeBase bodyNode;
            if (currentToken.type == TokenType.ARROW)
            {
                result.RegisterAdvancement();
                Advance();

                bodyNode = result.Register(ParseExpression());
                if (result.error != null) return result;

                return result.Success(new FunctionDefinitionNode(varNameToken, argTokens.ToArray(), bodyNode, true));
            }

            if (currentToken.type != TokenType.NEWLINE)
                return result.Failure(new InvalidSyntaxError("Expected '->' or newline", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            bodyNode = result.Register(ParseStatements());
            if (result.error != null) return result;

            if (!currentToken.Matches(TokenType.KEYWORD, "end"))
                return result.Failure(new InvalidSyntaxError("Expected 'END'", currentToken.startPosition));

            result.RegisterAdvancement();
            Advance();

            return result.Success(new FunctionDefinitionNode(varNameToken, argTokens.ToArray(), bodyNode, false));
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
