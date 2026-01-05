using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    public class ContactController : Controller
    {
        private readonly IGenericRepository<Contact> _repo;

        public ContactController(IGenericRepository<Contact> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var contact = _repo.GetFirstOrDefault();
            return View(contact);
        }
    }
}