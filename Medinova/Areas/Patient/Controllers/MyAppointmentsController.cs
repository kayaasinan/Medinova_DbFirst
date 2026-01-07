using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Services;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Patient.Controllers
{
    [AuthorizeRole(Roles.Patient)]
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

            var appointments = _context.Appointments.Where(x => x.UserId == userId).OrderByDescending(x => x.AppointmentDate).ThenByDescending(x => x.AppointmentTime).ToList();

            if (appointments.Any(x => x.IsActive == false))
            {
                TempData["DoctorCancelled"] = true;
            }

            return View(appointments);
        }

        public ActionResult MakePassive(int id)
        {
            int userId = (int)Session["UserId"];

            var appointment = _context.Appointments
                .FirstOrDefault(x => x.AppointmentId == id && x.UserId == userId);

            if (appointment == null)
                return RedirectToAction("Index");

            appointment.IsActive = false;
            _context.SaveChanges();

            LogService.Info
                (
                    "Hasta randevusunu pasif hale getirdi",
                    "MakePassive",
                    "MyAppointments",
                    userId,
                    Session["UserName"]?.ToString(),
                    "Patient"
                );
            return RedirectToAction("Index");
        }
    }
}