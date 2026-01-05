using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{

    [RouteArea("Admin")]
    public class AboutsController : Controller
    {
        private readonly IGenericRepository<About> _repo;

        public AboutsController(IGenericRepository<About> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var abouts = _repo.GetAll();
            return View(abouts);
        }

        public ActionResult UpdateAbout(int id)
        {
            var about = _repo.GetById(id);
            return View(about);
        }

        [HttpPost]
        public ActionResult UpdateAbout(About model)
        {
            var about = _repo.GetById(model.AboutId);

            about.Title = model.Title;
            about.Description = model.Description;
            about.ImageUrl = model.ImageUrl;

            _repo.Update(about);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteAbout(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}