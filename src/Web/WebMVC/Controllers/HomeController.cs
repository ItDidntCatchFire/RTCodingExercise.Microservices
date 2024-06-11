using IntegrationEvents;
using MassTransit;
using RTCodingExercise.Microservices.Models;
using System.Diagnostics;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestClient<SearchEvent> _publishEndpoint;

        public HomeController(IRequestClient<SearchEvent> publishEndpoint, ILogger<HomeController> logger)
        {
            _logger = logger;
            _publishEndpoint  = publishEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var plates = await GetPlates();

			return View(plates);
        }

        public async Task<ICollection<Plate>> GetPlates()
        {
            var response = await _publishEndpoint.GetResponse<ICollection<Plate>>(new
            {
                PageNumber = 0
            });

            var temp = response.Message;

            return temp;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}