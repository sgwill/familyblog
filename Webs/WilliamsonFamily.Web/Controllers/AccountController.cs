using System;
using System.Web.Mvc;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Injectables
        public IMembershipService MembershipService { get; set; }

        private void EnsureInjectables()
        {
            if (MembershipService == null) MembershipService = new AccountMembershipService();
        }
        #endregion

        #region Constructor
        public AccountController()
        {
            // TODO: Remove me with spring
            EnsureInjectables();
        }
        #endregion

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            return View("Login");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string userName, string password, bool? rememberMe, string returnUrl)
        {
            if (String.IsNullOrEmpty(userName))
                ModelState.AddModelError("username", "You must specify a username.");
            if (String.IsNullOrEmpty(password))
                ModelState.AddModelError("password", "You must specify a password.");
            if (!MembershipService.Login(userName, password, rememberMe ?? false))
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");

            if (!ModelState.IsValid)
                return View();

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            MembershipService.Logout();
            return RedirectToAction("Index", "Home");
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult Profile(string user)
        //{
        //    // TODO: Is this a valid user?
        //    // TODO: Grab current profile information

        //    return View("Index");
        //}
    }
}