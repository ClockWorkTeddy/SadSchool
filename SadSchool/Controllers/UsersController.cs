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
            return View(new RegisterViewModel() { RolesForDisplay = GetRolesForDisplay() });
        }

        private List<string> GetRolesForDisplay()
        {
            var roles = _context.Roles.ToList();
            
            return roles.Where(_ => _.Name != "admin").Select(_ => _.Name).ToList();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var roleName = model.Role.ToString();

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
            var users = _context.Users.ToList();

            return View("~/Views/Stuff/Users.cshtml", users);
        }
    }
}
