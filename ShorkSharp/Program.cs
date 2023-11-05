namespace ShorkSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.Write("\U0001f988>");
                string input = Console.ReadLine();
                Console.WriteLine(Run(input));
                Console.WriteLine();
            }
        }

        internal static string Run(string input, string filename = "<stdin>")
        {
            (Token[] tokens, ShorkError? error) = new Lexer(input, filename).Lex();
            if (error != null)
            {
                return error.ToString();
            }

            return string.Join<Token>(", ", tokens);
        }
    }
}