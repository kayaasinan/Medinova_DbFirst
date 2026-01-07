using Medinova.ML.Models;
using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medinova.Services
{
    public class DepartmentForecastMlService
    {
        private readonly MedinovaContext _context;

        public DepartmentForecastMlService(MedinovaContext context)
        {
            _context = context;
        }

        public List<DepartmentForecastDto> ForecastNextMonth()
        {
        
            var rawData = _context.Appointments
                .Where(a => a.AppointmentDate != null)
                .Join(_context.Doctors,
                      a => a.DoctorId,
                      d => d.DoctorId,
                      (a, d) => new
                      {
                          a.AppointmentDate,
                          d.DepartmentId
                      })
                .Join(_context.Departments,
                      ad => ad.DepartmentId,
                      dep => dep.DepartmentId,
                      (ad, dep) => new
                      {
                          dep.DepartmentId,
                          dep.Name,
                          ad.AppointmentDate
                      })
                .ToList();

      
            var monthlyData = rawData
                .GroupBy(x => new
                {
                    x.DepartmentId,
                    x.Name,
                    Month = new DateTime(
                        x.AppointmentDate.Value.Year,
                        x.AppointmentDate.Value.Month,
                        1)
                })
                .Select(g => new
                {
                    g.Key.DepartmentId,
                    g.Key.Name,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            // 3️⃣ Son ay
            var lastMonth = monthlyData.Max(x => x.Month);

            var lastMonthByDept = monthlyData
                .Where(x => x.Month == lastMonth)
                .ToList();

            int totalLastMonth = lastMonthByDept.Sum(x => x.Count);

            var result = new List<DepartmentForecastDto>();

            foreach (var dept in lastMonthByDept)
            {
                var history = monthlyData
                    .Where(x => x.DepartmentId == dept.DepartmentId)
                    .OrderBy(x => x.Month)
                    .Select(x => x.Count)
                    .ToList();

                double adjustmentFactor = 1.0;

                if (history.Count >= 2)
                {
                    int last = history[history.Count-1];
                    int prev = history[history.Count-2];

                    double rate = prev == 0 ? 0 : (double)(last - prev) / prev;

                  
                    adjustmentFactor = 1 + Math.Max(-0.15, Math.Min(0.15, rate));
                }

                double predicted = dept.Count * adjustmentFactor;

                result.Add(new DepartmentForecastDto
                {
                    DepartmentName = dept.Name,
                    PredictedCount = (int)Math.Round(predicted)
                });
            }

       
            int predictedTotal = result.Sum(x => x.PredictedCount);

            foreach (var item in result)
            {
                item.PredictedCount =
                    (int)Math.Round(
                        (double)item.PredictedCount / predictedTotal * totalLastMonth
                    );
            }

            return result;
        }
    }
}
