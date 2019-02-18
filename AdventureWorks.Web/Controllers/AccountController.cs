using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AdventureWorks.Web.Helpers;
using AdventureWorks.Web.Models;
using Microsoft.AspNet.Identity;

namespace AdventureWorks.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IWebApiProxy webApiProxy) : base(webApiProxy)
        {
        }

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("ViewPerson", "Person", new { id = Id });
            }

            return View();
        }

        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await WebApiProxy.ValidateUserPassword(model.Email, model.Password);
            switch (result.Status)
            {
                case HttpStatusCode.OK:
                    {
                        var user = result.Data;
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Hash, user.UserId.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Thumbprint, user.AccessToken)
                        };

                        var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                        var ctx = Request.GetOwinContext();
                        var authenticationManager = ctx.Authentication;
                        authenticationManager.SignIn(id);
                        FormsAuthentication.SetAuthCookie(user.UserName, false);

                        return RedirectToAction("ViewPerson", "Person", new { id = user.Id });
                    }

                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
    }
}