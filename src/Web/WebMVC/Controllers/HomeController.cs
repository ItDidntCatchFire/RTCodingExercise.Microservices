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

        public async Task<IActionResult> Index(int pageNumber = 0)
        {
            var plates = await GetPlates(pageNumber);

			return View(plates);
        }
        
        [HttpPost("Reserve")]
        public IActionResult Reserve(Guid id)
        {
            return Ok("Reserved");
        }

        [HttpPost("Purchase")]
        public IActionResult Purchase(Guid id)
        { 
            return Ok("Purchase");
        }

        public async Task<ICollection<Plate>> GetPlates(int pageNumber)
        {
            var response = await _publishEndpoint.GetResponse<Plate[]>(new
            {
                PageNumber = pageNumber,
                
            });

            var message = response.Message;

            return message;
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