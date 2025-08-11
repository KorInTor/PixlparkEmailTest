using Microsoft.AspNetCore.Mvc;
using PixlparkEmailTest.Services;

namespace PixlparkEmailTest.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ICodeSenderService _codeSenderService;
        private readonly ICodeManager _codeManager;

        public RegistrationController(ICodeSenderService codeSenderService, ICodeManager codeManager)
        {
            _codeSenderService = codeSenderService;
            _codeManager = codeManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterSendCode(string email)
        {
            _codeSenderService.SendCodeAsync(email, _codeManager.GenerateCodeAndCache(email));
            ViewBag.Email = email;
            return View("Register");
        }

        [HttpPost]
        public IActionResult RegisterVerifyCode(string email, string code)
        {
            var cachedCode = _codeManager.TryGetCode(email);
            if (cachedCode != code)
            {
                ViewBag.Email = email;
                ViewBag.Error = true;
                return View("Register");
            }
            _codeManager.RemoveCode(email);
            return View("Succes");
        }
    }
}
