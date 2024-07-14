using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.DataContext;

namespace WeatherAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var weatherData = await _context.WeatherRecords.ToListAsync();
            return View(weatherData);
        }

        public async Task<IActionResult> Graph()
        {
            var weatherData = await _context.WeatherRecords.ToListAsync();
            return View(weatherData);
        }
    }
}
