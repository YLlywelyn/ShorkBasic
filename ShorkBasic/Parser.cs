using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorkBasic
{
    internal class Parser
    {
        internal static NodeBase Parse(IEnumerable<Token> tokens)
        {
            return new Parser(tokens).DoParse();
        }

        protected int index { get; set; }
        protected Token[] tokens { get; set; }
        protected Token currentToken { get; set; }

        protected Parser(IEnumerable<Token> tokens)
        {
            this.tokens = tokens.ToArray();
            this.index = -1;
            Advance();
        }

        protected void Advance()
        {
            index++;
            currentToken = (index < tokens.Length) ? this.tokens[index] : null;
        }

        protected NodeBase DoParse()
        {
            NodeBase rootNode = ParseExpression();
            if (currentToken.type == TokenType.EOF)
            {
                return rootNode;
            }
            else
                throw new InvalidSyntaxError(currentToken.startPosition, currentToken.endPosition,
                                            "Expected '+', '-', '*', '/', '^', '==', '!=', '<', '>', <=', '>=', 'AND' or 'OR'");
        }

        protected NodeBase ParseAtom()
        {
            Token token = currentToken;

            if ((TokenType.INT | TokenType.FLOAT).HasFlag(token.type))
            {
                Advance();
                return new NumberNode(token);
            }

            else if (token.type == TokenType.IDENTIFIER)
            {
                Advance();
                return new VarAccessNode(token);
            }

            else if (token.type == TokenType.LPAREN)
            {
                Advance();
                NodeBase expression = ParseExpression();

                if (currentToken.type == TokenType.RPAREN)
                {
                    Advance();
                    return expression;
                }
                else
                {
                    throw new InvalidSyntaxError(token.startPosition, token.endPosition,
                                                "Expected ')'");
                }
            }

            else
            {
                throw new InvalidSyntaxError(token.startPosition, token.endPosition,
                                            "Expected int, float, '+', '-' or '('");
            }
        }

        protected NodeBase ParseExponent()
        {
            return ParseBinaryOperation(ParseAtom, TokenType.EXPONENT, ParseFactor);
        }

        protected NodeBase ParseFactor()
        {
            Token token = currentToken;

            if ((TokenType.PLUS | TokenType.MINUS).HasFlag(token.type))
            {
                Advance();
                NodeBase factor = ParseFactor();
                return new UnaryOperationNode(token, factor);
            }
            else
            {
                return ParseExponent();
            }
        }

        protected NodeBase ParseTerm()
        {
            return ParseBinaryOperation(ParseFactor, (TokenType.MULTIPLY | TokenType.DIVIDE));
        }

        protected NodeBase ParseArithmeticExpression()
        {
            return ParseBinaryOperation(ParseTerm, (TokenType.PLUS | TokenType.MINUS));
        }

        protected NodeBase ParseComparisonExpression()
        {
            if (currentToken.Matches(TokenType.KEYWORD, "not"))
            {
                Token opToken = currentToken;
                Advance();
                NodeBase node = ParseComparisonExpression();
                return new UnaryOperationNode(opToken, node);
            }

            return ParseBinaryOperation(ParseArithmeticExpression, (TokenType.DOUBLE_EQUALS
                                                                  | TokenType.NOT_EQUALS
                                                                  | TokenType.LESS_THAN
                                                                  | TokenType.LESS_THAN_OR_EQUAL
                                                                  | TokenType.GREATER_THAN
                                                                  | TokenType.GREATER_THAN_OR_EQUAL));
        }

        protected NodeBase ParseExpression()
        {
            if (currentToken.Matches(TokenType.KEYWORD, "var"))
            {
                Advance();

                if (currentToken.type != TokenType.IDENTIFIER)
                    throw new InvalidSyntaxError(currentToken.startPosition, currentToken.endPosition,
                                                    "Expected identifier");
                Token varNameToken = currentToken;
                Advance();

                if (currentToken.type != TokenType.EQUALS)
                    throw new InvalidSyntaxError(currentToken.startPosition, currentToken.endPosition,
                                                    "Expected '='");
                Advance();

                NodeBase expr = ParseExpression();
                return new VarAssignNode(varNameToken, expr);
            }
            else
            {
                return ParseBinaryOperation(ParseComparisonExpression, new (TokenType, dynamic)[] { (TokenType.KEYWORD, "and"),
                                                                                                    (TokenType.KEYWORD, "or")});
            }
        }

        protected delegate NodeBase BinaryOperationDelegate();
        protected NodeBase ParseBinaryOperation(BinaryOperationDelegate leftFunc, TokenType operators)
        {
            return ParseBinaryOperation(leftFunc, operators, leftFunc);
        }
        protected NodeBase ParseBinaryOperation(BinaryOperationDelegate leftFunc, TokenType operators, BinaryOperationDelegate rightFunc)
        {
            NodeBase leftNode = leftFunc();

            while (operators.HasFlag(currentToken.type))
            {
                Token opToken = currentToken;
                Advance();
                NodeBase rightNode = rightFunc();

                leftNode = new BinaryOperationNode(leftNode, opToken, rightNode);
            }

            return leftNode;
        }
        protected NodeBase ParseBinaryOperation(BinaryOperationDelegate leftFunc, (TokenType, dynamic)[] tokenTypes)
        {
            return ParseBinaryOperation(leftFunc, tokenTypes, leftFunc);
        }
        protected NodeBase ParseBinaryOperation(BinaryOperationDelegate leftFunc, (TokenType, dynamic)[] tokenTypes, BinaryOperationDelegate rightFunc)
        {
            NodeBase leftNode = leftFunc();

            while (tokenTypes.Contains((currentToken.type, currentToken.value)))
            {
                Token opToken = currentToken;
                Advance();
                NodeBase rightNode = rightFunc();

                leftNode = new BinaryOperationNode(leftNode, opToken, rightNode);
            }

            return leftNode;
        }
    }
}
