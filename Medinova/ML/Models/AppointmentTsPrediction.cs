
using Microsoft.ML.Data;

namespace Medinova.ML.Models
{
    public class AppointmentTsPrediction
    {
        [VectorType(7)]
        public float[] ForecastedCounts { get; set; }
    }
}