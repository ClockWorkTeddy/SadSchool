using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SadSchool.Models;
using SadSchool.ViewModels;
using System.Data;

namespace SadSchool.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthDbContext _context;

        public UsersController(UserManager<IdentityUser> userManager, 
                               RoleManager<IdentityRole> roleManager,
                               AuthDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml", 
                        new RegisterViewModel() { RolesForDisplay = GetRolesForDisplay() });
        }

        private List<string> GetRolesForDisplay()
        {
            var roles = _context.Roles.ToList();
            
            return roles.Where(_ => _.Name != "admin").Select(_ => _.Name).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!(User.Identity.IsAuthenticated && User.IsInRole("admin")))
                return View("Index");

            var roleName = model.RoleName;

            if (ModelState.IsValid)
            {
                if (_roleManager.Roles.First(_ => _.Name == roleName) == null)
                {
                    var role = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(role);
                }

                IdentityUser user = new IdentityUser { UserName = model.UserName };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, roleName);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RolesProcessing()
        {
            var roles = _context.Roles.ToList();

            return View("~/Views/Users/Roles.cshtml", roles);
        }

        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            return View("~/Views/Users/AddRole.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(NewRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.RoleName);

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("RolesProcessing", "Users");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = _context.Roles.First(_ => _.Id == id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("RolesProcessing", "Users");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("RolesProcessing", "Users");
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var usersViewModel = GetUsersListForView();

            return View("~/Views/Stuff/Users.cshtml", usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _context.Users.First(_ => _.Id == id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Users", "Users");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Users", "Users");
        }

        private List<UsersViewModel> GetUsersListForView()
        {
            List<UsersViewModel> result = new List<UsersViewModel>();

            foreach (var user in _context.Users)
            {
                result.Add(new UsersViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()!.ToString()
                });
            }

            return result;
        }
    }
}
