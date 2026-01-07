namespace Medinova.ML.Models
{
    public class DoctorMonthlyForecastDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public int PredictedAppointmentCount { get; set; }
    }
}