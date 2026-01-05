using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    public class MedicalPackageController : Controller
    {
        private readonly IGenericRepository<MedicalPackage> _repo;

        public MedicalPackageController(IGenericRepository<MedicalPackage> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var packages = _repo.GetWhere(x => x.IsActive == true);
            return View(packages);
        }
    }
}