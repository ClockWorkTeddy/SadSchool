// <copyright file="UsersController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers
{
    using System.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Controllers.Contracts;
    using SadSchool.Models;
    using SadSchool.Services;
    using SadSchool.ViewModels;

    /// <summary>
    /// Processes requests for user data.
    /// </summary>
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AuthDbContext context;
        private readonly INavigationService navigationService;
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userManager">User manager instance.</param>
        /// <param name="roleManager">Role manager instance.</param>
        /// <param name="context">DB context instance.</param>
        /// <param name="navigationService">Processes the "Back" button operations.</param>
        /// <param name="authService">Service processes user authorization check.</param>
        public UsersController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AuthDbContext context,
            INavigationService navigationService,
            IAuthService authService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
            this.navigationService = navigationService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the register view.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "Register" view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            return this.View(
                "~/Views/Account/Register.cshtml",
                new RegisterViewModel() { RolesForDisplay = this.GetRolesForDisplay() });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">View model with data.</param>
        /// <returns><see cref="IActionResult"/> for the "Index" view in case of no rights or success or
        ///     for the "Register" form in case of failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.authService.IsAdmin(this.User))
            {
                return this.View("Index");
            }

            var roleName = model.RoleName;

            if (this.ModelState.IsValid)
            {
                if (this.roleManager.Roles.First(_ => _.Name == roleName) == null)
                {
                    var role = new IdentityRole(roleName ?? string.Empty);
                    await this.roleManager.CreateAsync(role);
                }

                IdentityUser user = new IdentityUser { UserName = model.UserName };

                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.userManager.AddToRoleAsync(user, roleName ?? string.Empty);

                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// Processes the roles.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "Roles" view.</returns>
        [HttpGet]
        public async Task<IActionResult> RolesProcessing()
        {
            var roles = this.context.Roles.ToList();

            this.navigationService.RefreshBackParams(this.RouteData);

            return await Task.FromResult(this.View("~/Views/Users/Roles.cshtml", roles));
        }

        /// <summary>
        /// Gets the form for adding a new role.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "AddRole" view.</returns>
        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            this.navigationService.RefreshBackParams(this.RouteData);

            return await Task.FromResult(this.View("~/Views/Users/AddRole.cshtml"));
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="model">View model with data.</param>
        /// <returns><see cref="IActionResult"/> for the "AddRole" view.</returns>
        [HttpPost]
        public async Task<IActionResult> AddRole(NewRoleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var role = new IdentityRole(model.RoleName);

                var result = await this.roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("RolesProcessing", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="id">Deleted role id.</param>
        /// <returns><see cref="IActionResult"/> for the redirect to action "RolesProcessing".</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = this.context.Roles.First(_ => _.Id == id);

            if (role != null)
            {
                var result = await this.roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("RolesProcessing", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return this.RedirectToAction("RolesProcessing", "Users");
        }

        /// <summary>
        /// Gets the users view.
        /// </summary>
        /// <returns><see cref="IActionResult"/> for the "Users" view.</returns>
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var usersViewModel = this.GetUsersListForView();

            this.navigationService.RefreshBackParams(this.RouteData);

            return await Task.FromResult(this.View("~/Views/Stuff/Users.cshtml", usersViewModel));
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">Deleted user id.</param>
        /// <returns><see cref="IActionResult"/> for the redirect to action "Users".</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = this.context.Users.First(_ => _.Id == id);

            if (user != null)
            {
                var result = await this.userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("Users", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return this.RedirectToAction("Users", "Users");
        }

        private List<string?> GetRolesForDisplay()
        {
            var roles = this.context.Roles.ToList();

            return roles.Where(_ => _.Name != "admin").Select(_ => _.Name).ToList();
        }

        private List<UsersViewModel> GetUsersListForView()
        {
            List<UsersViewModel> result = new List<UsersViewModel>();

            foreach (var user in this.context.Users)
            {
                result.Add(new UsersViewModel()
                {
                    Id = user.Id,
                    UserName = user?.UserName,
                    Role = this.userManager.GetRolesAsync(user!).Result.FirstOrDefault()?.ToString(),
                });
            }

            return result;
        }
    }
}
