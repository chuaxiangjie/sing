using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

            //You get the user's first and last name below:
            
            ViewBag.Name = userClaims?.FindFirst("name")?.Value;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void LoginSingPass()
        {
            int i = 0;

            HttpContext.GetOwinContext().Authentication.Challenge(
          new AuthenticationProperties { RedirectUri = "/" },
          OpenIdConnectAuthenticationDefaults.AuthenticationType);

        }
    }
}