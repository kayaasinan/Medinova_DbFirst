using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using Medinova.Services;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    [AuthorizeRole(Roles.Admin)]
    public class MedicalServicesController : Controller
    {
        private readonly IGenericRepository<MedicalService> _repo;

        public MedicalServicesController(IGenericRepository<MedicalService> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var services = _repo.GetAll();
            return View(services);
        }

        public ActionResult CreateMedicalService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMedicalService(MedicalService model)
        {
            _repo.Add(model);
            LogService.Info
                (
                    "Admin yeni bir medikal hizmet ekledi",
                    "Create",
                    "MedicalServices",
                    (int?)Session["UserId"],
                    Session["UserName"]?.ToString(),
                    "Admin"
                );
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateMedicalService(int id)
        {
            var service = _repo.GetById(id);
            return View(service);
        }

        [HttpPost]
        public ActionResult UpdateMedicalService(MedicalService model)
        {
            var service = _repo.GetById(model.MedicalServiceId);

            service.Title = model.Title;
            service.Description = model.Description;
            service.Icon = model.Icon;

            _repo.Update(service);

            LogService.Info
                (
                    "Admin medikal hizmeti güncelledi",
                    "Update",
                    "MedicalServices",
                    (int?)Session["UserId"],
                    Session["UserName"]?.ToString(),
                    "Admin"
                );
            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteMedicalService(int id)
        {
            _repo.Delete(id);
            LogService.Info
                (
                    "Admin medikal hizmeti sildi",
                    "Delete",
                    "MedicalServices",
                    (int?)Session["UserId"],
                    Session["UserName"]?.ToString(),
                    "Admin"
                );
            return RedirectToAction(nameof(Index));
        }
    }
}