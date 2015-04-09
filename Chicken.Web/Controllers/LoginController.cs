using System.Web.Mvc;
using System.Web.Security;
using artgelina.web.Models;

namespace Chicken.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (model.Login == appSettings["login"] && model.Password == appSettings["password"])
            {
                FormsAuthentication.SetAuthCookie("admin", true);
                return RedirectToAction("Index", "Admin");
            }

            this.ModelState.AddModelError("", "Incorrect login or password.");
            return View("Index", model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
