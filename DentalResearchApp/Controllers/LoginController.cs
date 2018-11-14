using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IContext _context;
        public LoginController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Signup")]
        public async Task<IActionResult> SignUp(string Id)
        {
            var countryList = Countries.CountryList();
            var model = new SignUpModel
            {
                Country = countryList.OrderBy(a => a).ToList(),
                Errors = false
            };

            var manager = _context.ManagerFactory.CreateSignupLinkManager();
            var linkModel = await manager.GetLink(Id);

            if(linkModel != null)
                return View(model);
            else
                return Unauthorized();
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(SignUpModel signupModel)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();
            var signupLinkManager = _context.ManagerFactory.CreateSignupLinkManager();

            if (userManager.GetAllUsers().Result.Select(x => x.Email).Contains(signupModel.Email))
            {
                signupModel.Errors = true;
                return View("SignUp", signupModel);
            }

            var usermodel = new UserModel()
            {
                Country = signupModel.Country.FirstOrDefault(),
                Email = signupModel.Email,
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Institution = signupModel.Institution,
                Role = Role.Researcher
            };

            signupModel.Errors = false;

            var salt = Salt.Create();
            var hash = Hash.Create(signupModel.Password, salt);

            var login = new UserCredentials() {UserName = signupModel.Email, Hash = hash, Salt = salt};

            await userManager.CreateUser(usermodel, login);

            await signupLinkManager.DeleteLink(signupModel.LinkId);

            return RedirectToAction("Login");
        }

        [HttpGet("ForgotMyPasswordModal")]
        public IActionResult ForgotMyPasswordModal()
        {
            return View();
        }

        [HttpGet("ForgotMyPassword")]
        public async Task<IActionResult> ForgotMyPassword(string email)
        {
            var userManager = _context.ManagerFactory.CreateUserManager();
            var user = await userManager.GetUserModel(email);

            if (user != null)
            {
                var randomString = Path.GetRandomFileName();
                randomString = randomString.Replace(".", "");

                var salt = Salt.Create();
                var hash = Hash.Create(randomString, salt);

                var credentials = new UserCredentials() { UserName = email, Hash = hash, Salt = salt };
                await userManager.UpdateUserCredentials(credentials);

                var mailHelper = new MailHelper();
                mailHelper.SendMail(email, "Forgotten Password",
                    "You have asked to reset your password because it was forgotten.\n" +
                    "Your new password is: " + randomString + "\n" +
                    "Kind Regards\n" +
                    "AU Dent MailBot");

                ViewBag.Message = "Your password has been reset. Your new password has been sent to: " + email;
                return View("Login");
            }

            ViewBag.Message = "No user found for email: "+ email;
            return View("Login");
        }
    }
}