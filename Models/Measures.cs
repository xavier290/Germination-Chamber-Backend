
namespace ModelsMQTT_Server 
{
    public class Measurement
    {
        public int Id { get; set; }
        public int IdSensor { get; set; }
        public string? Topic { get; set; }
        public string? Message { get; set; }
        public DateTime Fecha { get; set; }
    }
}