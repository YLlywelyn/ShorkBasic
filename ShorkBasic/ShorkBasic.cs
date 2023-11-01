namespace ShorkBasic
{
    public static class ShorkBasic
    {
        public static string Run(string input, string filename = "<stdin>")
        {
            Token[] tokens = Lexer.Lex(input, filename);

            NodeBase rootNode = Parser.Parse(tokens);

            return string.Empty+rootNode.ToString();
        }
    }
}