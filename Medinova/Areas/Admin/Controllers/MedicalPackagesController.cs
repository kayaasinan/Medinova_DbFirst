using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    [AuthorizeRole(Roles.Admin)]
    public class MedicalPackagesController : Controller
    {
        private readonly IGenericRepository<MedicalPackage> _repo;

        public MedicalPackagesController(IGenericRepository<MedicalPackage> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var packages = _repo.GetAll();
            return View(packages);
        }

        public ActionResult CreateMedicalPackage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMedicalPackage(MedicalPackage model)
        {
            _repo.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateMedicalPackage(int id)
        {
            var package = _repo.GetById(id);
            return View(package);
        }

        [HttpPost]
        public ActionResult UpdateMedicalPackage(MedicalPackage model)
        {
            var package = _repo.GetById(model.MedicalPackagesId);

            package.PackageName = model.PackageName;
            package.Feature1 = model.Feature1;
            package.Feature2 = model.Feature2;
            package.Feature3 = model.Feature3;
            package.Feature4 = model.Feature4;
            package.Price = model.Price;
            package.ImageUrl = model.ImageUrl;
            package.IsActive = true;

            _repo.Update(package);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteMedicalPackage(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}