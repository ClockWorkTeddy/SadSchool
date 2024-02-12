// <copyright file="AccountController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.ViewModels;

    /// <summary>
    /// Manages account operations.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">User sign in manager.</param>
        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Go to login dialog.
        /// </summary>
        /// <returns>Login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return this.View(new LoginViewModel());
        }

        /// <summary>
        /// Processes login procedure.
        /// </summary>
        /// <param name="model"><see cref="LoginViewModel"/> DTO.</param>
        /// <returns>Login result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    this.ModelState.AddModelError("Password", "Think twice, little friend.");
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// Logout procedure.
        /// </summary>
        /// <returns>Redirects to Home/Index action.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Index", "Home");
        }
    }
}
