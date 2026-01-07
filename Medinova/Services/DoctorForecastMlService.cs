using Medinova.ML.Models;
using Medinova.Models;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Medinova.Services
{
    public class DoctorForecastMlService
    {
        private readonly MedinovaContext _context;
        private readonly MLContext _mlContext;

    
        public DoctorForecastMlService(MedinovaContext context)
        {
            _context = context;
            _mlContext = new MLContext(seed: 1);
        }

        public List<DoctorMonthlyForecastDto> ForecastNextMonthByDoctor()
        {
            var doctors = _context.Doctors.ToList();
            var results = new List<DoctorMonthlyForecastDto>();

            foreach (var doctor in doctors)
            {
                var rawData = _context.Appointments
                    .Where(x => x.DoctorId == doctor.DoctorId && x.AppointmentDate != null)
                    .GroupBy(x => DbFunctions.TruncateTime(x.AppointmentDate))
                    .OrderBy(g => g.Key)
                    .Select(g => new AppointmentTsData
                    {
                        AppointmentCount = g.Count()
                    })
                    .ToList();

         
                if (rawData.Count < 21)
                    continue;

                var dataView = _mlContext.Data.LoadFromEnumerable(rawData);

                var pipeline = _mlContext.Forecasting.ForecastBySsa(
                    outputColumnName: nameof(AppointmentTsPrediction.ForecastedCounts),
                    inputColumnName: nameof(AppointmentTsData.AppointmentCount),
                    windowSize: 14,
                    seriesLength: rawData.Count,
                    trainSize: rawData.Count - 7,
                    horizon: 30);

                var model = pipeline.Fit(dataView);

                var engine = model.CreateTimeSeriesEngine
                    <AppointmentTsData, AppointmentTsPrediction>(_mlContext);

                var forecast = engine.Predict();

                var nextMonthTotal = forecast.ForecastedCounts.Sum();

                results.Add(new DoctorMonthlyForecastDto
                {
                    DoctorId = doctor.DoctorId,
                    FullName = doctor.FullName,
                    PredictedAppointmentCount = (int)Math.Round(nextMonthTotal)
                });
            }

            return results
                .OrderByDescending(x => x.PredictedAppointmentCount)
                .ToList();
        }
    }
}
