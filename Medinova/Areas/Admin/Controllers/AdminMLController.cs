using Medinova.Consts;
using Medinova.Filters;
using Medinova.ML.Models;
using Medinova.Models;
using Medinova.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    [AuthorizeRole(Roles.Admin)]
    public class AdminMLController : Controller
    {
        private readonly AppointmentMlService _appointmentMlService;
        private readonly DoctorForecastMlService _doctorForecastMlService;
        private readonly DepartmentAnalyticsService _departmentAnalyticsService;
        private readonly DepartmentForecastMlService _departmentForecastMlService;
        private readonly MlDashboardCardService _mlDashboardCardService;

        public AdminMLController(AppointmentMlService appointmentMlService, DoctorForecastMlService doctorForecastMlService, DepartmentAnalyticsService departmentAnalyticsService, DepartmentForecastMlService departmentForecastMlService, MlDashboardCardService mlDashboardCardService)
        {
            _appointmentMlService = appointmentMlService;
            _doctorForecastMlService = doctorForecastMlService;
            _departmentAnalyticsService = departmentAnalyticsService;
            _departmentForecastMlService = departmentForecastMlService;
            _mlDashboardCardService = mlDashboardCardService;
        }


        public ActionResult Index()
        {
            var forecast = _appointmentMlService.ForecastNextWeek();

            var result = Enumerable.Range(0, 7)
                .Select(i => new
                {
                    Day = DateTime.Today.AddDays(i + 1).ToString("dd MMM"),
                    Count = Math.Round(forecast[i], 0)
                })
                .ToList();

            ViewBag.ChartData = JsonConvert.SerializeObject(result);
            ViewBag.Average = Math.Round(result.Average(x => x.Count), 1);

            return View();
        }

        public PartialViewResult MonthlyForecast()
        {
            var data = _doctorForecastMlService.ForecastNextMonthByDoctor();
            return PartialView(data);
        }
        public PartialViewResult DepartmentDonut()
        {
            var data = _departmentAnalyticsService.GetDepartmentAppointmentDistribution();
            return PartialView(data);
        }
        public PartialViewResult DepartmentPieForecast()
        {
            var data = _departmentForecastMlService.ForecastNextMonth();
            return PartialView(data);
        }
        public PartialViewResult MlDashboardCards()
        {
            var cards = new List<DashboardCardDto>
            {
                _mlDashboardCardService.GetTopDoctorNextMonth(),
                _mlDashboardCardService.GetTopDepartmentNextMonth(),
                _mlDashboardCardService.GetTopPatientProbability(),
                _mlDashboardCardService.GetTotalPatientForecast()
            };

            return PartialView(cards);
        }
    }
}
