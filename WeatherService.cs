using Newtonsoft.Json;
using WeatherAPI.DataContext;
using WeatherAPI.Models.WeatherAPI;

namespace WeatherAPI.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, ApplicationDbContext context, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;          
        }

        public async Task FetchWeatherDataAsync()
        {
            var cities = new List<(string Country, string City)>
            {
                ("Latvia", "Sigulda"),
                ("Latvia", "Riga"),
                ("France", "Paris"),
                ("France", "Antibes")
            };

            string apiKey = GetApiKey();

            foreach (var city in cities)
            {
                try
                {
                    var response = await _httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={city.City}&appid={apiKey}&units=metric");
                    _logger.LogInformation($"API Response for {city.City}, {city.Country}: {response}");
                    var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

                    if (weatherResponse != null)
                    {
                        var weatherData = new Models.Weather
                        {
                            WeatherApiCityId = weatherResponse.Id,
                            Country = weatherResponse.Sys.Country,
                            City = city.City,
                            Temperature = weatherResponse.Main.Temp,
                            LastUpdateTime = DateTime.UtcNow
                        };

                        _context.WeatherRecords.Add(weatherData);
                    }
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError(e, $"Error fetching weather data for {city.City}, {city.Country}");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An unexpected error occurred.");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving weather data to the database.");
            }
        }

        private string GetApiKey()
        {
            var apiKey = Environment.GetEnvironmentVariable("WeatherApiApiKey");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("Cannot retrieve WeatherApiApiKey.");
            }

            return apiKey;
        }
    }  
}
