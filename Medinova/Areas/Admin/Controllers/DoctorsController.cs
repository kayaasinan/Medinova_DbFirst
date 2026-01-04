using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly MedinovaContext _context;
        public DoctorsController(MedinovaContext context)
        {
            _context = context;
        }
        private void GetDepatment()
        {
            var departments = _context.Departments.ToList();
            ViewBag.departments = (from department in departments
                                   select new SelectListItem
                                   {
                                       Text = department.Name,
                                       Value = department.DepartmentId.ToString()
                                   }).ToList();
        }
        public ActionResult Index()
        {
            var doctors = _context.Doctors.ToList();
            return View(doctors);
        }
        public ActionResult CreateDoctor()
        {
            GetDepatment();
            return View();
        }
        [HttpPost]
        public ActionResult CreateDoctor(Models.Doctor model)
        {
            _context.Doctors.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateDoctor(int id)
        {
            GetDepatment();
            var doctor = _context.Doctors.Find(id);
            return View(doctor);
        }

        [HttpPost]
        public ActionResult UpdateDoctor(Models.Doctor model)
        {
            var doctor = _context.Doctors.Find(model.DoctorId);

            doctor.FullName = model.FullName;
            doctor.Description = model.Description;
            doctor.ImageUrl = model.ImageUrl;
            doctor.DepartmentId= model.DepartmentId;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteDoctor(int id)
        {
            var doctor = _context.Doctors.Find(id);
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}