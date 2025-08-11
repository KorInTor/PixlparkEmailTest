using System.Collections.Concurrent;

namespace PixlparkEmailTest.Services
{
    public class RegistrationCodeManager : ICodeManager
    {
        private ICodeGenerator _codeGenerator;

        //Лучше использовать бд для этого и подключать сервис бд но для задания думаю хватит и простого словаря
        private ConcurrentDictionary<string, string> _cachedEmailCodes = [];

        public RegistrationCodeManager(ICodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        public string GenerateCodeAndCache(string email)
        {
            string code = _codeGenerator.GenerateAndReturn();
            _cachedEmailCodes[email] = code;
            return code;
        }

        public void RemoveCode(string email)
        {
            _cachedEmailCodes.TryRemove(email, out _);
        }

        public string? TryGetCode(string email)
        {
            _cachedEmailCodes.TryGetValue(email, out string? code);
            return code;
        }
    }
}
