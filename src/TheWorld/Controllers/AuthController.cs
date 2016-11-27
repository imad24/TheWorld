using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<WorldUser> _signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm,string returnurl)
        {
            if (ModelState.IsValid)
            {
                var signInResult =await _signInManager.PasswordSignInAsync(vm.Username
                    , vm.Password
                    , true,false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnurl)) return Redirect(returnurl);
                    else return RedirectToAction("Trips", "App");
                }
                else
                {
                    ModelState.AddModelError("","Username or password incorrect");
                }

            }
            

            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }
    }
}
