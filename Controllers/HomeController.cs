using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OldDependency.Services;
using System;
using System.Threading.Tasks;

namespace OldDependency.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IImageService service;

        public HomeController(SignInManager<IdentityUser> signInManager, IImageService service)
        {
            this.signInManager = signInManager;
            this.service = service;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            var imagesOfUsers = this.service.GetAll();

            return View(imagesOfUsers);
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await this.signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Upload");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
        }

        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/upload")]
        [Authorize]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Route("/upload")]
        [Authorize]
        public IActionResult Upload(IFormFile fileFromUser)
        {
            try
            {
                this.service.ExtractAndStoreImages(this.User.Identity.Name, fileFromUser);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View();
            }

            return RedirectToAction("Index");
        }
    }
}