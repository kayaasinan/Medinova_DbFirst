using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{

    [RouteArea("Admin")]
    public class AboutsController : Controller
    {
        private readonly MedinovaContext _context;
        public AboutsController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var abouts = _context.Abouts.ToList();
            return View(abouts);
        }

        public ActionResult UpdateAbout(int id)
        {
            var about = _context.Abouts.Find(id);
            return View(about);
        }

        [HttpPost]
        public ActionResult UpdateAbout(About model)
        {
            var about = _context.Abouts.Find(model.AboutId);

            about.Title = model.Title;
            about.Description = model.Description;
            about.ImageUrl = model.ImageUrl;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
      
            _context.Abouts.Remove(value);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}