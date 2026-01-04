using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    public class MedicalServiceController : Controller
    {
        private readonly MedinovaContext _context;
        public MedicalServiceController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var services = _context.MedicalServices.ToList();
            return View(services);
        }
    }
}