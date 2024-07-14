namespace WeatherAPI.Models
{
    public class Weather
    {
        public int Id { get; set; }

        public int WeatherApiCityId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }    

        public double Temperature { get; set; }    
        
        public DateTime LastUpdateTime { get; set; }
    }
}
