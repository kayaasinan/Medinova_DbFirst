using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class AboutItemsController : Controller
    {
        private readonly MedinovaContext _context;
        public AboutItemsController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var aboutItems = _context.AboutItems.ToList();
            return View(aboutItems);
        }

        public ActionResult CreateAboutItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAboutItem(AboutItem model)
        {
            _context.AboutItems.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateAboutItem(int id)
        {
            var aboutItem = _context.AboutItems.Find(id);
            return View(aboutItem);
        }

        [HttpPost]
        public ActionResult UpdateAboutItem(AboutItem model)
        {
            var aboutItem = _context.AboutItems.Find(model.AboutItemId);

            aboutItem.Title = model.Title;
            aboutItem.Icon = model.Icon;
       
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteAboutItem(int id)
        {
            var value = _context.AboutItems.Find(id);
      
            _context.AboutItems.Remove(value);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}