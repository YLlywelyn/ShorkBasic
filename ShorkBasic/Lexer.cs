﻿using System.Collections.Generic;
using System.Linq;

namespace ShorkBasic
{
    internal class Lexer
    {
        readonly string[] KEYWORDS = {
            "var"
        };
        readonly char[] DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        readonly char[] DIGITS_DOT = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
        readonly char[] WHITESPACE = { ' ', '\t', '\n', '\r' };
        readonly char[] LETTERS = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        internal static Token[] Lex(string input, string filename)
        {
            return new Lexer(input, filename).DoLex();
        }
        
        protected string input { get; set; }
        protected Position position { get; set; }
        protected char? currentChar { get; set; }
        
        protected Lexer(string input, string filename)
        {
            this.input = input;
            this.position = new Position(filename);
            Advance();
        }
        
        protected void Advance()
        {
            position.Advance(currentChar == 'n');
            currentChar = (position.index < input.Length) ? input[position.index] : null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Token[] DoLex()
        {
            List<Token> tokens = new List<Token>();

            while (currentChar != null)
            {
                if (WHITESPACE.Contains((char)currentChar))
                {
                    Advance();
                    continue;
                }

                else if (DIGITS.Contains((char)currentChar))
                {
                    tokens.Add(MakeNumber());
                }

                else if (LETTERS.Contains((char)currentChar))
                {
                    tokens.Add(MakeIdentifier());
                }

                else if (currentChar == '+')
                {
                    tokens.Add(new Token(TokenType.PLUS, position));
                    Advance();
                }
                else if (currentChar == '-')
                {
                    tokens.Add(new Token(TokenType.MINUS, position));
                    Advance();
                }
                else if (currentChar == '*')
                {
                    tokens.Add(new Token(TokenType.MULTIPLY, position));
                    Advance();
                }
                else if (currentChar == '/')
                {
                    tokens.Add(new Token(TokenType.DIVIDE, position));
                    Advance();
                }
                else if (currentChar == '^')
                {
                    tokens.Add(new Token(TokenType.EXPONENT, position));
                    Advance();
                }
                else if (currentChar == '(')
                {
                    tokens.Add(new Token(TokenType.LPAREN, position));
                    Advance();
                }
                else if (currentChar == ')')
                {
                    tokens.Add(new Token(TokenType.RPAREN, position));
                    Advance();
                }

                else if (currentChar == '!')
                {
                    tokens.Add(MakeNotEquals());
                }
                else if (currentChar == '=')
                {
                    tokens.Add(MakeEquals());
                }
                else if (currentChar == '<')
                {
                    tokens.Add(MakeLessThan());
                }
                else if (currentChar == '>')
                {
                    tokens.Add(MakeGreaterThan());
                }

                else
                    throw new InvalidCharacterError(position, position,
                                        string.Format("Invalid character '{0}' found.", currentChar));
            }

            tokens.Add(new Token(TokenType.EOF, position));
            return tokens.ToArray();
        }

        Token MakeNumber()
        {
            Position startPosition = position.Copy();
            string numString = string.Format("{0}", (char)currentChar);
            bool hasDecimalPoint = false;
            Advance();

            while (currentChar != null && DIGITS_DOT.Contains((char)currentChar))
            {
                numString += (char)currentChar;

                if (currentChar == '.')
                {
                    if (hasDecimalPoint) throw new InvalidCharacterError(startPosition, position,
                                                    "Number cannot have more than one decimal point.");
                    else hasDecimalPoint = true;
                }

                Advance();
            }

            TokenType ttype = hasDecimalPoint ? TokenType.FLOAT : TokenType.INT;
            if (ttype == TokenType.INT)
                return new Token(ttype, startPosition, position, int.Parse(numString));
            else
                return new Token(ttype, startPosition, position, decimal.Parse(numString));
        }

        Token MakeIdentifier()
        {
            Position startPosition = position.Copy();
            string idString = string.Format("{0}", (char)currentChar);
            Advance();

            while (currentChar != null && LETTERS.Concat(DIGITS).Concat(new char[] { '_' }).Contains((char)currentChar))
            {
                idString += (char)currentChar;
                Advance();
            }

            TokenType ttype = KEYWORDS.Contains(idString.ToLower()) ? TokenType.KEYWORD : TokenType.IDENTIFIER;
            return new Token(ttype, startPosition, position, idString);
        }

        Token MakeNotEquals()
        {
            Position startPosition = position.Copy();
            Advance();

            if ((char)currentChar == '=')
            {
                Advance();
                return new Token(TokenType.NOT_EQUALS, startPosition, position);
            }
            else
            {
                Advance();
                throw new ExpectedCharacterError(startPosition, position, "Expected '=' after '!'");
            }
        }

        Token MakeEquals()
        {
            Position startPosition = position.Copy();
            TokenType ttype = TokenType.EQUALS;
            Advance();

            if ((char)currentChar == '=')
            {
                Advance();
                ttype = TokenType.DOUBLE_EQUALS;
            }

            return new Token(ttype, startPosition, position);
        }

        Token MakeLessThan()
        {
            Position startPosition = position.Copy();
            TokenType ttype = TokenType.LESS_THAN;
            Advance();

            if ((char)currentChar == '=')
            {
                Advance();
                ttype = TokenType.LESS_THAN_OR_EQUAL;
            }

            return new Token(ttype, startPosition, position);
        }

        Token MakeGreaterThan()
        {
            Position startPosition = position.Copy();
            TokenType ttype = TokenType.GREATER_THAN;
            Advance();

            if ((char)currentChar == '=')
            {
                Advance();
                ttype = TokenType.GREATER_THAN_OR_EQUAL;
            }

            return new Token(ttype, startPosition, position);
        }
    }
}