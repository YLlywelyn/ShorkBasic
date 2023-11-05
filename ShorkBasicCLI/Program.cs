using System.IO;
using ShorkBasic;

namespace ShorkBasicCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    string text = File.ReadAllText(args[0]);
                    string filename = Path.GetFileName(args[0]);
                    
                    try
                    {
                        Console.WriteLine(ShorkBasic.ShorkBasic.Run(text, filename));
                    }
                    catch (ShorkError e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    Console.WriteLine("FATAL ERROR: Could not find file '{0}'", args[0]);
                }
            }
            else
            {
                RunInteractive();
            }
        }

        static void RunInteractive()
        {
            while (true)
            {
                Console.Write("\U0001f988>");
                string input = Console.ReadLine();

                try
                {
                    Console.WriteLine(ShorkBasic.ShorkBasic.Run(input, sharedContext: true));
                }
                catch (ShorkError e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    Console.WriteLine();
                }
            }
        }
    }
}