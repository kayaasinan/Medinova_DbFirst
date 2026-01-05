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
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Default");

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginDto model)
        {
            var user = _context.Users.Include("UserRoles.Role").FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı");
                return View();
            }

            var role = user.UserRoles.FirstOrDefault()?.Role?.RoleName;

            if (role == null)
            {
                ModelState.AddModelError("", "Kullanıcı rolü bulunamadı");
                return View();
            }

            FormsAuthentication.SetAuthCookie(user.UserName, false);

            Session["UserId"] = user.UserId;
            Session["UserName"] = user.UserName;
            Session["Role"] = role;

            return RedirectToAction("Index", "Default");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index","Default");
        }
    }
}