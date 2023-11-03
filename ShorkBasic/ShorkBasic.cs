namespace ShorkBasic
{
    public static class ShorkBasic
    {
        static SymbolTable sharedSymbolTable = new SymbolTable();

        public static string Run(string input, string filename = "<stdin>", bool sharedContext = false)
        {
            Token[] tokens = Lexer.Lex(input, filename);
#if DEBUG_OUTPUT
            Console.WriteLine(string.Format("DEBUG: Tokens: {0}", string.Join(',', (IEnumerable<Token>)tokens)));
#endif

            NodeBase rootNode = Parser.Parse(tokens);
#if DEBUG_OUTPUT
            Console.WriteLine(string.Format("DEBUG: RootNode: {0}", rootNode));
#endif

            Context context = new Context("<program>", null, sharedContext ? sharedSymbolTable : null);
            ShorkObject result = Interpreter.VisitNode(rootNode, context);
#if DEBUG_OUTPUT
            Console.WriteLine(string.Format("DEBUG: Result: {0}", result));
#endif

            return result.ToString();
        }
    }
}