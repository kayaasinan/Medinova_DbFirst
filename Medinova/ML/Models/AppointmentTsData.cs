
using Microsoft.ML.Data;

namespace Medinova.ML.Models
{
    public class AppointmentTsData
    {
        [LoadColumn(0)]
        public float AppointmentCount { get; set; }
    }
}