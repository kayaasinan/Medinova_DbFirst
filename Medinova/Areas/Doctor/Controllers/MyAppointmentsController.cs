using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
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

            var appointments = _context.Appointments.Where(x => x.DoctorId == doctor.DoctorId).OrderBy(x => x.AppointmentDate).ThenBy(x => x.AppointmentTime).ToList();

            return View(appointments);
        }
        public ActionResult ToggleStatus(int id)
        {
            int userId = (int)Session["UserId"];

            var doctor = _context.Doctors.FirstOrDefault(x => x.UserId == userId);
            if (doctor == null)
                return RedirectToAction("Index");

            var appointment = _context.Appointments.FirstOrDefault(x => x.AppointmentId == id && x.DoctorId == doctor.DoctorId);

            if (appointment == null)
                return RedirectToAction("Index");

            appointment.IsActive = !(appointment.IsActive ?? true);
            _context.SaveChanges();

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