using ShorkBasic;

namespace ShorkBasicCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Write("\U0001f988>");
                string input = Console.ReadLine();

                Console.WriteLine(ShorkBasic.ShorkBasic.Run(input));
            }
        }
    }
}