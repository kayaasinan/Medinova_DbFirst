using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class MedicalServicesController : Controller
    {
        private readonly MedinovaContext _context;

        public MedicalServicesController(MedinovaContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var services = _context.MedicalServices.ToList();
            return View(services);
        }
        public ActionResult CreateMedicalService()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMedicalService(MedicalService model)
        {
            _context.MedicalServices.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateMedicalService(int id)
        {
            var service = _context.MedicalServices.Find(id);
            return View(service);
        }

        [HttpPost]
        public ActionResult UpdateMedicalService(MedicalService model)
        {
            var service = _context.MedicalServices.Find(model.MedicalServiceId);

            service.Title = model.Title;
            service.Description = model.Description;
            service.Icon = model.Icon;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteMedicalService(int id)
        {
            var service = _context.MedicalServices.Find(id);
            _context.MedicalServices.Remove(service);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}