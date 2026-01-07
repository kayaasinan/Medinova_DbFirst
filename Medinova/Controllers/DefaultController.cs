using Medinova.Consts;
using Medinova.DTOs;
using Medinova.Models;
using Medinova.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        private readonly MedinovaContext _context;
        public DefaultController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult DefaultAppointment()
        {
            var departments = _context.Departments.ToList();

            ViewBag.departments = departments.Select(department => new SelectListItem
            {
                Text = department.Name,
                Value = department.DepartmentId.ToString()
            }).ToList();

            var dateList = new List<SelectListItem>();

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Now.AddDays(i);

                dateList.Add(new SelectListItem
                {
                    Text = date.ToString("dd.MMMM.dddd"),
                    Value = date.ToString("yyyy-MM-dd")
                });
            }

            ViewBag.dateList = dateList;

            return PartialView();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateAppointment()
        {
           
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

     
            return View();
        }
        [HttpPost]
        public ActionResult CreateAppointment(Appointment appointment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                LogService.Info
                    (
                        "Giriş yapmadan randevu oluşturma denemesi yapıldı",
                        "CreateAppointment",
                        "Default",
                        null,
                        null,
                        null
                    );
                return RedirectToAction("Login", "Account");
            }

            appointment.UserId = (int)Session["UserId"];
            appointment.IsActive = true;

            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            LogService.Info
                (
                    "Hasta yeni bir randevu oluşturdu",
                    "CreateAppointment",
                    "Default",
                    appointment.UserId,
                    Session["UserName"]?.ToString(),
                    "Patient"
                 );
            return RedirectToAction("Index");
        }
        public JsonResult GetDoctorsByDepartmentId(int departmentId)
        {
            var doctors = _context.Doctors.Where(x => x.DepartmentId == departmentId)
                                          .Select(doctor => new SelectListItem
                                          {
                                              Text = doctor.FullName,
                                              Value = doctor.DoctorId.ToString()
                                          }).ToList();
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAvailableHours(DateTime selectedDate, int doctorId)
        {
            var bookedTimes = _context.Appointments.Where(x => x.DoctorId == doctorId && x.AppointmentDate == selectedDate && x.IsActive == true).Select(x => x.AppointmentTime).ToList();

            var dtoList = new List<AppointmentAvailabilityDto>();

            foreach (var hour in Times.AppointmentHours)
            {
                dtoList.Add(new AppointmentAvailabilityDto
                {
                    Time = hour,
                    IsBooked = bookedTimes.Contains(hour)
                });
            }

            return Json(dtoList, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Testimonial()
        {
            var testimonial = _context.Testimonials.ToList();
            return PartialView(testimonial);
        }
        public PartialViewResult Doctors()
        {
            var doctors = _context.Doctors.ToList();
            return PartialView(doctors);
        }
        public PartialViewResult MedicalPackages()
        {
            var packages = _context.MedicalPackages.Where(x => x.IsActive == true).ToList();
            return PartialView(packages);
        }
        public PartialViewResult MedicalServices()
        {
            var services = _context.MedicalServices.ToList();
            return PartialView(services);
        }
        public PartialViewResult About()
        {
            var abouts = _context.Abouts.ToList();
            return PartialView(abouts);
        }
        public PartialViewResult AboutItem()
        {
            var aboutItems = _context.AboutItems.ToList();
            return PartialView(aboutItems);
        }
        public PartialViewResult Banner()
        {
            var banners = _context.Banners.ToList();
            return PartialView(banners);
        }
        public PartialViewResult HealthAiSuggestion()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> GetHealthAiSuggestion(string userText)
        {
            var service = new AiHealthSuggestionService();
            var result = await service.GetDepartmentSuggestionAsync(userText);
            LogService.Info
                (
                    "AI sağlık önerisi alındı",
                    "GetHealthAiSuggestion",
                    "HealthAi"
                );

            return Json(
                new { success = true, data = result },
                JsonRequestBehavior.AllowGet
            );
        }
    }
}