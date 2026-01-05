using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    [AuthorizeRole(Roles.Admin)]
    public class DoctorsController : Controller
    {
        private readonly IGenericRepository<Models.Doctor> _doctorRepo;
        private readonly IGenericRepository<Department> _departmentRepo;

        public DoctorsController(IGenericRepository<Models.Doctor> doctorRepo, IGenericRepository<Department> departmentRepo)
        {
            _doctorRepo = doctorRepo;
            _departmentRepo = departmentRepo;
        }

        private void GetDepartment()
        {
            ViewBag.departments = _departmentRepo.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.DepartmentId.ToString()
                }).ToList();
        }

        public ActionResult Index()
        {
            var doctors = _doctorRepo.GetAll();
            return View(doctors);
        }

        public ActionResult CreateDoctor()
        {
            GetDepartment();
            return View();
        }

        [HttpPost]
        public ActionResult CreateDoctor(Models.Doctor model)
        {
            _doctorRepo.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateDoctor(int id)
        {
            GetDepartment();
            var doctor = _doctorRepo.GetById(id);
            return View(doctor);
        }

        [HttpPost]
        public ActionResult UpdateDoctor(Models.Doctor model)
        {
            var doctor = _doctorRepo.GetById(model.DoctorId);


            doctor.FullName = model.FullName;
            doctor.Description = model.Description;
            doctor.ImageUrl = model.ImageUrl;
            doctor.DepartmentId = model.DepartmentId;

            _doctorRepo.Update(doctor);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteDoctor(int id)
        {
            _doctorRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
