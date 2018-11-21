using DentalResearchApp.Models;
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

        [HttpGet("EditUserModal")]
        public IActionResult EditUserModal(string userMail)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();
            var user = userManager.GetUserModel(userMail).Result;

            return View(user);
        }

        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromForm]UserModel userModel)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();
            var user = await userManager.GetUserModel(userModel.Email);

            if (user != null)
            {
                user.Role = userModel.Role;
                await userManager.UpdateUserData(user);
            }

            return RedirectToAction("Admin");
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();

            var user = await userManager.GetUserModel(email);
            var creds = await userManager.GetUserCredentials(email);

            var a = userManager.DeleteUser(user, creds);

            return RedirectToAction("Admin", controllerName: "Admin");
        }
    }
}
