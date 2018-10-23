using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Signup")]
        public IActionResult SignUp()
        {
            var countryList = countries.CountryList();
            var model = new SignUpModel
            {
                Country = countryList.OrderBy(a => a).ToList(),
                Errors = false
            };

            return View(model);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(SignUpModel signupModel)
        {
            var userManager = new UserManager();
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

            return RedirectToAction("Login");
        }
    }

    public class countries
    {
        public static List<string> CountryList()
        {
            //Creating Dictionary
            var cultureList = new List<string>();

            //getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                //creating the object of RegionInfo class
                RegionInfo getRegionInfo = new RegionInfo(getCulture.Name);
                //adding each country Name into the Dictionary
                if (!(cultureList.Contains(getRegionInfo.EnglishName)))
                {
                    cultureList.Add(getRegionInfo.EnglishName);
                }
            }
            //returning country list
            return cultureList;
        }
    }

}