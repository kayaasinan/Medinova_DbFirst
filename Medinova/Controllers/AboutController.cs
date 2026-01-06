using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        private readonly IGenericRepository<About> _aboutRepo;
        private readonly IGenericRepository<AboutItem> _aboutItemRepo;

        public AboutController(IGenericRepository<About> aboutRepo, IGenericRepository<AboutItem> aboutItemRepo)
        {
            _aboutRepo = aboutRepo;
            _aboutItemRepo = aboutItemRepo;
        }

        public ActionResult Index()
        {
            var abouts = _aboutRepo.GetAll();
            return View(abouts);
        }
        public PartialViewResult AboutItemPage()
        {
            var aboutItems = _aboutItemRepo.GetAll();
            return PartialView(aboutItems);
        }
    }
}