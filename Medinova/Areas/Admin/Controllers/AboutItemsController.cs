using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    [AuthorizeRole(Roles.Admin)]
    public class AboutItemsController : Controller
    {
        private readonly IGenericRepository<AboutItem> _repo;

        public AboutItemsController(IGenericRepository<AboutItem> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var aboutItems = _repo.GetAll();
            return View(aboutItems);
        }

        public ActionResult CreateAboutItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAboutItem(AboutItem model)
        {
            _repo.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateAboutItem(int id)
        {
            var aboutItem = _repo.GetById(id);
            return View(aboutItem);
        }

        [HttpPost]
        public ActionResult UpdateAboutItem(AboutItem model)
        {
            var entity = _repo.GetById(model.AboutItemId);

            entity.Title = model.Title;
            entity.Icon = model.Icon;

            _repo.Update(entity);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteAboutItem(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}