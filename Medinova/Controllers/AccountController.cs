using Medinova.DTOs;
using Medinova.Models;
using Medinova.Services;
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
            {
                LogService.Info(
                    "Giriş yapmış kullanıcı login sayfasına erişmeye çalıştı",
                    "Login-GET",
                    "Account",
                    (int?)Session["UserId"],
                    Session["UserName"]?.ToString(),
                    Session["Role"]?.ToString()
                );

                return RedirectToAction("Index", "Default");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDto model)
        {
            var user = _context.Users
                .Include("UserRoles.Role")
                .FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);


            if (user == null)
            {
                LogService.Info(
                    "Başarısız giriş denemesi - Hatalı kullanıcı adı veya şifre",
                    "Login-POST",
                    "Account",
                    null,
                    model.UserName,
                    null
                );

                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı");
                return View();
            }


            var role = user.UserRoles.FirstOrDefault()?.Role?.RoleName;
            if (role == null)
            {
                LogService.Info(
                    "Giriş başarısız - Kullanıcının rolü bulunamadı",
                    "Login-POST",
                    "Account",
                    user.UserId,
                    user.UserName,
                    null
                );

                ModelState.AddModelError("", "Kullanıcı rolü bulunamadı");
                return View();
            }
            FormsAuthentication.SetAuthCookie(user.UserName, false);

            Session["UserId"] = user.UserId;
            Session["UserName"] = user.UserName;
            Session["FullName"] = user.FirstName + " " + user.LastName;
            Session["Email"] = user.Email;
            Session["Role"] = role;

            LogService.Info(
                "Kullanıcı başarıyla giriş yaptı",
                "Login-POST",
                "Account",
                user.UserId,
                user.UserName,
                role
            );

            return RedirectToAction("Index", "Default");
        }


        public ActionResult Logout()
        {
            LogService.Info
                (
                   "Kullanıcı sistemden çıkış yaptı",
                   "Logout",
                   "Account",
                   (int?)Session["UserId"],
                   Session["UserName"]?.ToString(),
                   Session["Role"]?.ToString()
                );

            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Default");
        }
    }
}