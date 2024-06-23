using IntegrationEvents;
using MassTransit;
using RTCodingExercise.Microservices.Models;
using System.Diagnostics;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestClient<SearchEvent> _searchEndpoint;
        private readonly IPublishEndpoint _publishEndpoint;


        public HomeController(IRequestClient<SearchEvent> requestEndpoint, ILogger<HomeController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _searchEndpoint = requestEndpoint;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index(int pageNumber = 0)
        {
            var plates = await GetPlates(pageNumber);

			return View(plates);
        }
        
        [HttpPost("Reserve")]
        public async Task<IActionResult> Reserve(Guid id)
        {
            await _publishEndpoint.Publish<ReservePlate>(new
            {
                PlateId = id,
            });

            //TODO: persist the query data from previous index
            return RedirectToAction("Index");
        }

        [HttpPost("Purchase")]
        public async Task<IActionResult> Purchase(Guid id)
        {
            await _publishEndpoint.Publish<SellPlate>(new
            {
                PlateId = id,
            });

            //TODO: persist the query data from previous index
            return RedirectToAction("Index");
        }

        public async Task<ICollection<Plate>> GetPlates(int pageNumber)
        {
            var response = await _searchEndpoint.GetResponse<Plate[]>(new
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