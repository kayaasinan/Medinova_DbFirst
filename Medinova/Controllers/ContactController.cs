using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    public class ContactController : Controller
    {
        private readonly MedinovaContext _context;
        public ContactController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var contact = _context.Contacts.FirstOrDefault();
            return View(contact);
        }
    }
}