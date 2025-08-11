namespace PixlparkEmailTest.Services
{
    public interface ICodeSenderService
    {
        Task SendCodeAsync(string email, string code);
    }
}
