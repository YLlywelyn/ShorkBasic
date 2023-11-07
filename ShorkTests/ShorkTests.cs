namespace ShorkTests
{
    [TestClass]
    public class ShorkTests
    {
        [TestMethod]
        public void TestNumberTokenGeneration()
        {
            (Token[] tokens, ShorkError error) = new Lexer("123").Lex();
            
        }
    }
}