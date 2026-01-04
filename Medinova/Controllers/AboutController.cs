using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    public class AboutController : Controller
    {
        private readonly MedinovaContext _context;
        public AboutController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var abouts = _context.Abouts.ToList();
            return View(abouts);
        }
        public PartialViewResult AboutItemPage()
        {
            var aboutItems = _context.AboutItems.ToList();
            return PartialView(aboutItems);
        }
    }
}