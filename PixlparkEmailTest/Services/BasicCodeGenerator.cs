namespace PixlparkEmailTest.Services
{
    public class BasicCodeGenerator : ICodeGenerator
    {
        public string GenerateAndReturn()
        {
            int length = 5;

            var random = new Random();
            var chars = "0123456789";
            char[] codeChars = new char[(int)length];

            for (int i = 0; i < length; i++)
                codeChars[i] = chars[random.Next(chars.Length)];

            return new string(codeChars);
        }
    }
}
