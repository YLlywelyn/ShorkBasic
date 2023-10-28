using System.Collections.Generic;

namespace ShorkBasic
{
    internal class Lexer
    {
        public static Token[] LexText(string input, string filename)
        {
            return new Lexer(input, filename).DoLex();
        }
        
        protected string input { get; set; }
        
        protected Lexer(string input, string filename)
        {
            this.input = input;
            this.position = new Position(filename);
        }
        
        protected Token[] DoLex()
        {
            List<Token> tokens = new List<Token>();
            
            
            
            return tokens.ToArray();
        }
    }
}