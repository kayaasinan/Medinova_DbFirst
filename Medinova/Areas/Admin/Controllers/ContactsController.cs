using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IGenericRepository<Contact> _repo;

        public ContactsController(IGenericRepository<Contact> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var contacts = _repo.GetAll();
            return View(contacts);
        }

        public ActionResult CreateContact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateContact(Contact model)
        {
            _repo.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateContact(int id)
        {
            var contact = _repo.GetById(id);
            return View(contact);
        }

        [HttpPost]
        public ActionResult UpdateContact(Contact model)
        {
            var contact = _repo.GetById(model.ContactId);

            contact.MapUrl = model.MapUrl;
            contact.Title = model.Title;
            contact.Email = model.Email;
            contact.Phone = model.Phone;
            contact.Address = model.Address;

            _repo.Update(contact);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteContact(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}