namespace PixlparkEmailTest.Services
{
    public interface ICodeManager
    {
        public string GenerateCodeAndCache(string email);

        public string? TryGetCode(string email);

        public void RemoveCode(string email);
    }
}
