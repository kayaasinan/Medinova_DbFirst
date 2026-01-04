using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class BannersController : Controller
    {
        private readonly MedinovaContext _context;
        public BannersController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var banners = _context.Banners.ToList();
            return View(banners);
        }
        public ActionResult UpdateBanner(int id)
        {
            var banner = _context.Banners.Find(id);
            return View(banner);
        }

        [HttpPost]
        public ActionResult UpdateBanner(Banner model)
        {
            var banner = _context.Abouts.Find(model.BannerId);

            banner.Title = model.Title;
            banner.Description = model.Description;
            banner.ImageUrl = model.ImageUrl;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteBanner(int id)
        {
            var banner = _context.Banners.Find(id);
            _context.Banners.Remove(banner);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}