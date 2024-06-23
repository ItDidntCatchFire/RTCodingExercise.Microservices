using IntegrationEvents;
using MassTransit;
using RTCodingExercise.Microservices.Models;
using System.Diagnostics;
using WebMVC.Models;

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

        public async Task<IActionResult> Index(string orderby, int age = -1, string initials = "", int pageNumber = 0, string discountCode = "")
        {
            if (pageNumber < 0)
                pageNumber = 0;

            var plates = await GetPlates(pageNumber, age, initials, orderby, discountCode);

            var model = new HomeModel()
            {
                Plates = plates,
                Age = age > -1 ? age.ToString() : "",
                Initials = initials,
                PageNumber = pageNumber > -1 ? pageNumber.ToString() : "",
                OrderBy = orderby, //TODO: Figure out how to bind the drop down to what is selected
                DiscountCode = discountCode
            };

			return View(model);
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

        public async Task<ICollection<Plate>> GetPlates(int pageNumber, int age, string initials, string orderby, string discountCode)
        {
            var response = await _searchEndpoint.GetResponse<Plate[]>(new SearchEvent
            {
                PageNumber = pageNumber,
                Age = age,
                Initials = initials,
                OrderBy = orderby,
                DiscountCode = discountCode
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