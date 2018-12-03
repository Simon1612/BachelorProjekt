using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    public class ProfileController : Controller
    {
        private readonly IContext _context;

        public ProfileController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            var manager = _context.ManagerFactory.CreateUserManager();
            var user = await manager.GetUserModel(email);


            return View(user);
        }

        [HttpGet("ChangePasswordModal")]
        public IActionResult ChangePasswordModal(string email)
        {
            var model = new ChangePasswordModel() { Errors = false };

            return View(model);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            IActionResult response = Unauthorized();

            var userManager = _context.ManagerFactory.CreateUserManager();
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            var user = await userManager.Authenticate(new LoginModel { Username = email, Password = model.OldPassword });

            if (user == null)
            {
                var userModel = await userManager.GetUserModel(email);

                ViewBag.Message = "Old password was incorrect. No changes were made";
                return View("Profile", userModel);
            }


            var salt = Salt.Create();
            var hash = Hash.Create(model.NewPassword, salt);

            var credentials = new UserCredentials() { UserName = email, Hash = hash, Salt = salt };


            await userManager.UpdateUserCredentials(credentials);

            return RedirectToAction("Profile");
        }
    }
}