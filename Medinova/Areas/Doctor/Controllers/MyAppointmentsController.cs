using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Services;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Doctor.Controllers
{
    [AuthorizeRole(Roles.Doctor)]
    public class MyAppointmentsController : Controller
    {
        private readonly MedinovaContext _context;

        public MyAppointmentsController(MedinovaContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            int userId = (int)Session["UserId"];

            var doctor = _context.Doctors.FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
                return RedirectToAction("Index", "Default");

            var appointments = _context.Appointments.Where(x => x.DoctorId == doctor.DoctorId).OrderByDescending(x => x.AppointmentDate).ThenBy(x => x.AppointmentTime).ToList();

            return View(appointments);
        }
        public ActionResult ToggleStatus(int id)
        {
            int userId = (int)Session["UserId"];

            var doctor = _context.Doctors.FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
                return RedirectToAction("Index");

            var appointment = _context.Appointments
                .FirstOrDefault(x => x.AppointmentId == id && x.DoctorId == doctor.DoctorId);

            if (appointment == null)
                return RedirectToAction("Index");

            bool oldStatus = appointment.IsActive ?? true;
            appointment.IsActive = !oldStatus;
            _context.SaveChanges();

            LogService.Info(
                    appointment.IsActive == true
                    ? "Doktor randevuyu tekrar aktif hale getirdi"
                    : "Doktor randevuyu pasif hale getirdi",
                    "ToggleStatus",
                    "DoctorAppointments",
                    userId,
                    Session["UserName"]?.ToString(),
                    "Doctor"
            );

            return RedirectToAction("Index");
        }
        public JsonResult GetAppointmentDetail(int id)
        {
            int userId = (int)Session["UserId"];

            var doctor = _context.Doctors.FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            var appointment = _context.Appointments.FirstOrDefault(x => x.AppointmentId == id && x.DoctorId == doctor.DoctorId);

            if (appointment == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                appointment.FullName,
                appointment.Email,
                appointment.PhoneNumber,
                Date = appointment.AppointmentDate?.ToString("dd.MM.yyyy"),
                appointment.AppointmentTime,
                Status = appointment.IsActive == true ? "Aktif" : "Pasif"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}