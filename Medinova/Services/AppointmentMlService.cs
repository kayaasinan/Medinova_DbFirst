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
    public class AppointmentMlService
    {
        private readonly MedinovaContext _context;
        private readonly MLContext _mlContext;

        public AppointmentMlService(MedinovaContext context)
        {
            _context = context;
            _mlContext = new MLContext(seed: 1);
        }

        public float[] ForecastNextWeek()
        {
         
            var startDate = DateTime.Today.AddMonths(-3);

            var rawData = _context.Appointments
                .Where(x => x.AppointmentDate != null && x.AppointmentDate >= startDate)
                .GroupBy(x => DbFunctions.TruncateTime(x.AppointmentDate))
                .Select(g => new
                {
                    Date = g.Key.Value,
                    Count = g.Count()
                })
                .ToList();

          
            var filledData = new List<AppointmentTsData>();
            var minDate = rawData.Min(x => x.Date);
            var maxDate = rawData.Max(x => x.Date);

            for (var date = minDate; date <= maxDate; date = date.AddDays(1))
            {
                var dayData = rawData.FirstOrDefault(x => x.Date == date);
                filledData.Add(new AppointmentTsData
                {
                    AppointmentCount = dayData != null ? dayData.Count : 0
                });
            }

            if (filledData.Count < 21)
                return new float[7];

            var dataView = _mlContext.Data.LoadFromEnumerable(filledData);

            var pipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(AppointmentTsPrediction.ForecastedCounts),
                inputColumnName: nameof(AppointmentTsData.AppointmentCount),
                windowSize: 14,         
                seriesLength: filledData.Count,
                trainSize: filledData.Count - 7,
                horizon: 7);

            var model = pipeline.Fit(dataView);

            var engine = model.CreateTimeSeriesEngine
                <AppointmentTsData, AppointmentTsPrediction>(_mlContext);

            var forecast = engine.Predict();

            return forecast.ForecastedCounts;
        }
    }
}
