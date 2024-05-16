using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Web.Models;
using MyMicroservice.Web.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace MyMicroservice.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            if(!ModelState.IsValid)
            {
                return View(signInInput);
            }

            var response = await _identityService.SignIn(signInInput);

            if (!response.IsSuccesful)
            {
                response.Errors.ForEach(error =>
                {
                    ModelState.AddModelError(String.Empty, error);
                });

                return View();
            }
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
