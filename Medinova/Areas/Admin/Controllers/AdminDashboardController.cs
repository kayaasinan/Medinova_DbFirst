using Medinova.Consts;
using Medinova.Filters;
using Medinova.Models;
using Medinova.Services;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    [AuthorizeRole(Roles.Admin)]
    public class AdminDashboardController : Controller
    {
        private readonly MedinovaContext _context;

        public AdminDashboardController(MedinovaContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {

            ViewBag.TotalAppointments = _context.Appointments.Count();
            ViewBag.ActiveAppointments = _context.Appointments.Count(x => x.IsActive == true);
            var targetDate = DateTime.Today.AddDays(-3);
            ViewBag.TodayAppointments = _context.Appointments
                .Count(x => DbFunctions.TruncateTime(x.AppointmentDate) == targetDate);
            ViewBag.TotalUsers = _context.Users.Count();


            ViewBag.TodayEarnings = ViewBag.TodayAppointments * 100;
            ViewBag.MonthlyAppointments = _context.Appointments.Count(x => x.AppointmentDate.Value.Month == DateTime.Now.Month && x.AppointmentDate.Value.Year == DateTime.Now.Year);
            ViewBag.MonthlyDoctors = _context.Doctors.Count();


            var chartData = _context.Appointments
                .Where(x => x.AppointmentDate != null)
                .GroupBy(x => new
                {
                    x.AppointmentDate.Value.Year,
                    x.AppointmentDate.Value.Month
                })
                .Select(g => new
                {
                    Label = g.Key.Month + "/" + g.Key.Year,
                    Value = g.Count()
                })
                .OrderBy(x => x.Label)
                .Take(6)
                .ToList();

            ViewBag.ChartData = chartData;

            return View();
        }
    }
}
