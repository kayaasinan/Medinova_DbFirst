using Medinova.Models;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly MedinovaContext _context;

        public TestimonialsController(MedinovaContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var testimonials = _context.Testimonials.ToList();
            return View(testimonials);
        }
        public ActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTestimonial(Testimonial model)
        {
            _context.Testimonials.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateTestimonial(int id)
        {
            var testimonial = _context.Testimonials.Find(id);
            return View(testimonial);
        }

        [HttpPost]
        public ActionResult UpdateTestimonial(Testimonial model)
        {
            var testimonial = _context.Testimonials.Find(model.TestimonialId);

            testimonial.ImageUrl = model.ImageUrl;
            testimonial.FullName = model.FullName;
            testimonial.Title = model.Title;
            testimonial.Comment = model.Comment;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteTestimonial(int id)
        {
            var testimonial = _context.Testimonials.Find(id);
            _context.Testimonials.Remove(testimonial);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}