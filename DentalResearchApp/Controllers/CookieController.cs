using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class CookieController : Controller
    {
        private readonly IContext _context;
        public CookieController(IContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("VerifyLinkId")]
        public async Task<IActionResult> VerifyLinkId([FromBody] VerifyLinkIdModel model)
        {
            IActionResult response = Unauthorized();

            var manager = _context.ManagerFactory.CreateSurveyLinkManager();
            var link = await manager.GetLink(model.LinkId);

            if (link != null) // if link is verified
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, Role.Volunteer.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, link.ParticipantId.ToString()),
                    new Claim(ClaimTypes.Uri, link.LinkId),
                    new Claim(ClaimTypes.Name, link.SurveyName)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                response = Ok(new { message = link.SurveyName }); //Read response in view and redirect to survey url
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();

            var manager = _context.ManagerFactory.CreateUserManager();

            var user = await manager.Authenticate(login);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    //Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                    //The time at which the authentication ticket expires.A
                    // value set here overrides the ExpireTimeSpan option of
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    //Whether the authentication session is persisted across
                    // multiple requests.Required when setting the
                    // ExpireTimeSpan option of CookieAuthenticationOptions
                    // set with AddCookie.Also required when setting
                    // ExpiresUtc.

                    IssuedUtc = DateTime.UtcNow,
                    //The time at which the authentication ticket was issued.

                    //RedirectUri = "/Login"
                    //The full path or absolute URI to be used as an http
                    //redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                response = Ok(new { message = "Success" });
            }

            return response;
        }
    }
}