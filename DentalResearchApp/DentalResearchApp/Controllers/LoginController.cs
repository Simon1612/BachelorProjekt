using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

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
                Country = countryList.OrderBy(a => a).ToList()
            };
            return View(model);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(SignUpModel signupModel)
        {
            var userManager = new UserManager();

            var usermodel = new UserModel()
            {
                Country = signupModel.Country.FirstOrDefault(),
                Email = signupModel.Email,
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Institution = signupModel.Institution,
                Role = Role.Researcher
            };

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
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
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