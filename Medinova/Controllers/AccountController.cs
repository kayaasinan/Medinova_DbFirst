using Medinova.DTOs;
using Medinova.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Medinova.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
       
        private readonly MedinovaContext _context;

        public AccountController(MedinovaContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginDto model)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı..!");
                return View();
            }
            FormsAuthentication.SetAuthCookie(user.UserName, false);
            Session["userName"] = user.UserName;
            Session["fullName"] = user.FirstName + " " + user.LastName;

            return RedirectToAction("Index", "Abouts", new { area = "Admin" });
            
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");

        }
    }
}