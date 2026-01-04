using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    public class MedicalPackageController : Controller
    {
        private readonly MedinovaContext _context;
        public MedicalPackageController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var packages = _context.MedicalPackages.Where(x => x.IsActive == true).ToList();
            return View(packages);
        }
    }
}