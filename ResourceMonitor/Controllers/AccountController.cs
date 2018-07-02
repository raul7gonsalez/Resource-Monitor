namespace ResourceMonitor.Controllers
{
    using System.Web.Mvc;

    using ResourceMonitor.Authentification.Interfaces;
    using ResourceMonitor.Models;

    /// <summary>Контроллер аутентификации</summary>
    public class AccountController : Controller
    {
        public IAuthProvider AuthProvider { get; set; }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (AuthProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("List", "Admin"));
                }

                ModelState.AddModelError("", "Incorrect username or password");
                return View();
            }
            return View();
        }

        public ActionResult Logout(LoginModel model, string returnUrl)
        {
            AuthProvider.Logout();

            return RedirectToAction("ListResources", "Home");
        }
    }
}