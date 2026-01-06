using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    [AllowAnonymous]
    public class MedicalServiceController : Controller
    {
        private readonly IGenericRepository<MedicalService> _repo;

        public MedicalServiceController(IGenericRepository<MedicalService> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var services = _repo.GetAll();
            return View(services);
        }
    }
}