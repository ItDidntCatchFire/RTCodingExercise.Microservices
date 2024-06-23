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
        private readonly IPublishEndpoint _reserveEndpoint;


        public HomeController(IRequestClient<SearchEvent> publishEndpoint, ILogger<HomeController> logger, IPublishEndpoint reserveEndpoint)
        {
            _logger = logger;
            _searchEndpoint = publishEndpoint;
            _reserveEndpoint = reserveEndpoint;
        }

        public async Task<IActionResult> Index(int pageNumber = 0)
        {
            var plates = await GetPlates(pageNumber);

			return View(plates);
        }
        
        [HttpPost("Reserve")]
        public async Task<IActionResult> Reserve(Guid id)
        {
            await _reserveEndpoint.Publish<ReservePlate>(new
            {
                PlateId = id,
            });

            //persist the query data from previous index
            return RedirectToAction("Index");
        }

        [HttpPost("Purchase")]
        public async Task<IActionResult> Purchase(Guid id)
        {
            await _reserveEndpoint.Publish<SellPlate>(new
            {
                PlateId = id,
            });

            //persist the query data from previous index
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