using Medinova.Consts;
using Medinova.DTOs;
using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewBag.departments = (from department in departments
                                   select new SelectListItem
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
        [HttpPost]
        public ActionResult CreateAppointment(Appointment appointment)
        {
            appointment.IsActive = true;
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
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
            var bookedTimes = _context.Appointments.Where(x => x.DoctorId == doctorId && x.AppointmentDate == selectedDate).Select(x => x.AppointmentTime).ToList();

            var dtoList = new List<AppointmentAvailabilityDto>();

            foreach (var hour in Times.AppointmentHours)
            {
                var dto = new AppointmentAvailabilityDto();
                dto.Time = hour;

                if (bookedTimes.Contains(hour))
                {
                    dto.IsBooked = true;
                }
                else
                {
                    dto.IsBooked = false;
                }

                dtoList.Add(dto);
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
    }
}