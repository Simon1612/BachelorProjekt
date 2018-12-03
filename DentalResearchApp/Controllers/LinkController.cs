using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{

    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    public class LinkController : Controller
    {
        private readonly IContext _context;
        public LinkController(IContext context)
        {
            _context = context;
        }

        [HttpPost("sendSignupLink")]
        public async Task<IActionResult> SendSignupLink(InviteUserViewModel model)
        {
            var baseUrl = BaseUrlHelper.GetBaseUrl(Request);

            var manager = _context.ManagerFactory.CreateSignupLinkManager();

            await manager.SendLink(model.Email, baseUrl);

            return Redirect("/Admin");
        }
    }
}