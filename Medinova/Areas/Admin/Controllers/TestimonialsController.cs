using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly IGenericRepository<Testimonial> _repo;

        public TestimonialsController(IGenericRepository<Testimonial> repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var testimonials = _repo.GetAll();
            return View(testimonials);
        }

        public ActionResult CreateTestimonial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTestimonial(Testimonial model)
        {
            _repo.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult UpdateTestimonial(int id)
        {
            var testimonial = _repo.GetById(id);
            return View(testimonial);
        }

        [HttpPost]
        public ActionResult UpdateTestimonial(Testimonial model)
        {
            var testimonial = _repo.GetById(model.TestimonialId);

            testimonial.ImageUrl = model.ImageUrl;
            testimonial.FullName = model.FullName;
            testimonial.Title = model.Title;
            testimonial.Comment = model.Comment;

            _repo.Update(testimonial);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteTestimonial(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}