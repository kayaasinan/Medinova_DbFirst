using Medinova.ML.Models;
using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medinova.Services
{
    public class MlDashboardCardService
    {
        private readonly MedinovaContext _context;

        public MlDashboardCardService(MedinovaContext context)
        {
            _context = context;
        }


        public DashboardCardDto GetTopDoctorNextMonth()
        {
            var raw = _context.Appointments
                .Where(x => x.AppointmentDate != null)
                .Select(x => new
                {
                    x.DoctorId,
                    x.AppointmentDate
                })
                .ToList();

            var data = raw
                .GroupBy(x => new
                {
                    x.DoctorId,
                    Month = new DateTime(
                        x.AppointmentDate.Value.Year,
                        x.AppointmentDate.Value.Month,
                        1)
                })
                .Select(g => new
                {
                    g.Key.DoctorId,
                    g.Key.Month,
                    Count = g.Count()
                })
                .ToList();

            var prediction = data
                .GroupBy(x => x.DoctorId)
                .Select(g =>
                {
                    var ordered = g.OrderBy(x => x.Month).Select(x => x.Count).ToList();

                    int predicted;
                    if (ordered.Count < 2)
                        predicted = ordered.Last();
                    else
                        predicted = ordered.Last() + (ordered.Last() - ordered[ordered.Count - 2]);

                    return new
                    {
                        DoctorId = g.Key,
                        Predicted = Math.Max(0, predicted)
                    };
                })
                .OrderByDescending(x => x.Predicted)
                .First();

            var doctor = _context.Doctors.First(x => x.DoctorId == prediction.DoctorId);

            return new DashboardCardDto
            {
                Title = "Beklenen En Yoğun Doktor",
                MainValue = doctor.FullName,
                SubText = $"~{prediction.Predicted} randevu",
                BgClass = "bg-warning",
                IconClass = "mdi mdi-stethoscope"
            };
        }

        public DashboardCardDto GetTopDepartmentNextMonth()
        {
          
            var raw = _context.Appointments
                .Join(
                    _context.Doctors,
                    a => a.DoctorId,
                    d => d.DoctorId,
                    (a, d) => new
                    {
                        d.DepartmentId,
                        a.AppointmentDate
                    })
                .Where(x => x.AppointmentDate != null)
                .ToList();

      
            var data = raw
                .GroupBy(x => new
                {
                    x.DepartmentId,
                    Month = new DateTime(
                        x.AppointmentDate.Value.Year,
                        x.AppointmentDate.Value.Month,
                        1)
                })
                .Select(g => new
                {
                    g.Key.DepartmentId,
                    g.Key.Month,
                    Count = g.Count()
                })
                .ToList();

          
            var prediction = data
                .GroupBy(x => x.DepartmentId)
                .Select(g =>
                {
                    var ordered = g.OrderBy(x => x.Month).Select(x => x.Count).ToList();

                    int predicted;
                    if (ordered.Count < 2)
                        predicted = ordered.Last();
                    else
                        predicted = ordered.Last() + (ordered.Last() - ordered[ordered.Count - 2]);

                    return new
                    {
                        DepartmentId = g.Key,
                        Predicted = Math.Max(0, predicted)
                    };
                })
                .OrderByDescending(x => x.Predicted)
                .First();

            var department = _context.Departments
                .First(x => x.DepartmentId == prediction.DepartmentId);

            return new DashboardCardDto
            {
                Title = "Beklenen En Yoğun Bölüm",
                MainValue = department.Name,
                SubText = $"~{prediction.Predicted} randevu",
                BgClass = "bg-danger",
                IconClass = "mdi mdi-hospital-building"
            };
        }



        public DashboardCardDto GetTopPatientProbability()
        {
            var now = DateTime.Now;

            var scoredUser = _context.Appointments
                .GroupBy(x => x.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Count = g.Count(),
                    LastDate = g.Max(x => x.AppointmentDate)
                })
                .ToList()
                .Select(x =>
                {
                    int recencyScore = 0;
                    int days = (now - x.LastDate.Value).Days;

                    if (days <= 10)
                        recencyScore = 30;
                    else if (days <= 30)
                        recencyScore = 20;
                    else if (days <= 60)
                        recencyScore = 10;

                    int score = (x.Count * 2) + recencyScore;

                    return new
                    {
                        x.UserId,
                        Score = score
                    };
                })
                .OrderByDescending(x => x.Score)
                .First();

            var user = _context.Users.First(x => x.UserId == scoredUser.UserId);

            return new DashboardCardDto
            {
                Title = "Takip Gerektiren Hasta",
                MainValue = $"{user.FirstName} {user.LastName}",
                SubText = "Yüksek randevu ihtimali",
                BgClass = "bg-primary",
                IconClass = "mdi mdi-account-heart"
            };
        }


        public DashboardCardDto GetTotalPatientForecast()
        {
         
            var raw = _context.Appointments
                .Where(x => x.AppointmentDate != null)
                .Select(x => new
                {
                    x.UserId,
                    x.AppointmentDate
                })
                .ToList(); 

       
            var monthly = raw
                .GroupBy(x => new DateTime(
                    x.AppointmentDate.Value.Year,
                    x.AppointmentDate.Value.Month,
                    1))
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Select(x => x.UserId).Distinct().Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            int predicted;
            if (monthly.Count < 2)
            {
                predicted = monthly.Last().Count;
            }
            else
            {
                predicted =
                    monthly.Last().Count +
                    (monthly.Last().Count - monthly[monthly.Count - 2].Count);
            }

            return new DashboardCardDto
            {
                Title = "Beklenen Toplam Hasta",
                MainValue = predicted.ToString(),
                SubText = "Önümüzdeki ay",
                BgClass = "bg-success",
                IconClass = "mdi mdi-account-group"
            };
        }

    }
}
