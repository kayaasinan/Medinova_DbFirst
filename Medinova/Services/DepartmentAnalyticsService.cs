
using Medinova.ML.Models;
using Medinova.Models;
using System.Collections.Generic;
using System.Linq;

namespace Medinova.Services
{
    public class DepartmentAnalyticsService
    {
        private readonly MedinovaContext _context;

        public DepartmentAnalyticsService(MedinovaContext context)
        {
            _context = context;
        }

        public List<DepartmentDonutDto> GetDepartmentAppointmentDistribution()
        {
            var list = _context.Appointments.Join(_context.Doctors, a => a.DoctorId, d => d.DoctorId, (a, d) => new { a, d }).Join(_context.Departments, ad => ad.d.DepartmentId, dep => dep.DepartmentId, (ad, dep) => new { ad.a, dep }).GroupBy(x => x.dep.Name)
                .Select(g => new DepartmentDonutDto
                {
                    DepartmentName = g.Key,
                    AppointmentCount = g.Count()
                })
                .ToList();

            var total = list.Sum(x => x.AppointmentCount);

            foreach (var item in list)
            {
                item.AppointmentPercent = total == 0 ? 0 : (decimal)item.AppointmentCount * 100 / total;
            }
            return list;
        }
    }
}


