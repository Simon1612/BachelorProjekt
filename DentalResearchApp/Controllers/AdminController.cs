﻿using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IContext _context;
        public AdminController(IContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Admin()
        {
            var userManager = _context.ManagerFactory.CreateUserManager();
            var users = await userManager.GetAllUsers();
            var model = new InviteUserViewModel { UserList = users };

            return View(model);
        }

        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(string email, int newRole)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();
            var user = await userManager.GetUserModel(email);

            user.Role = (Role) newRole;
            await userManager.UpdateUserData(user);

            return RedirectToAction("Admin");
        }

        [HttpDelete("DeleteUser")]
        public async Task DeleteUser(string email)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();

            var user = await userManager.GetUserModel(email);
            var creds = await userManager.GetUserCredentials(email);

            await userManager.DeleteUser(user, creds);
        }
    }
}
