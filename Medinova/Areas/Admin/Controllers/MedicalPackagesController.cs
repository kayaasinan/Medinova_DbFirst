using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class MedicalPackagesController : Controller
    {
        private readonly MedinovaContext _context;

        public MedicalPackagesController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var packages = _context.MedicalPackages.ToList();
            return View(packages);
        }
        public ActionResult CreateMedicalPackage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMedicalPackage(MedicalPackage model)
        {
            _context.MedicalPackages.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateMedicalPackage(int id)
        {
            var package = _context.MedicalPackages.Find(id);
            return View(package);
        }

        [HttpPost]
        public ActionResult UpdateMedicalPackage(MedicalPackage model)
        {
            var package = _context.MedicalPackages.Find(model.MedicalPackagesId);

            package.PackageName = model.PackageName;
            package.Feature1 = model.Feature1;
            package.Feature2 = model.Feature2;
            package.Feature3 = model.Feature3;
            package.Feature4 = model.Feature4;
            package.Price = model.Price;
            package.ImageUrl = model.ImageUrl;
            package.IsActive = true;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteMedicalPackage(int id)
        {
            var package = _context.MedicalPackages.Find(id);
            _context.MedicalPackages.Remove(package);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}