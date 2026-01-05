using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class BannersController : Controller
    {
        private readonly IGenericRepository<Banner> _repo;

        public BannersController(IGenericRepository<Banner> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var banners = _repo.GetAll();
            return View(banners);
        }

        public ActionResult UpdateBanner(int id)
        {
            var banner = _repo.GetById(id);
            return View(banner);
        }

        [HttpPost]
        public ActionResult UpdateBanner(Banner model)
        {
            var banner = _repo.GetById(model.BannerId);

            banner.Title = model.Title;
            banner.Description = model.Description;
            banner.ImageUrl = model.ImageUrl;

            _repo.Update(banner);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteBanner(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}