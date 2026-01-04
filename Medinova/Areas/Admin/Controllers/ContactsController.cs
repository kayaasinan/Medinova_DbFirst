using Medinova.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class ContactsController : Controller
    {
        private readonly MedinovaContext _context;

        public ContactsController(MedinovaContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var contacts = _context.Contacts.ToList();
            return View(contacts);
        }
        public ActionResult CreateContact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateContact(Contact model)
        {
            _context.Contacts.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            return View(contact);
        }

        [HttpPost]
        public ActionResult UpdateContact(Contact model)
        {
            var contact = _context.Contacts.Find(model.ContactId);

            contact.MapUrl = model.MapUrl;
            contact.Title = model.Title;
            contact.Email = model.Email;
            contact.Phone = model.Phone;
            contact.Address = model.Address;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}