using RTCodingExercise.Microservices.Models;
using System.Diagnostics;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var plates = GetPlates();


			return View(plates);
        }

        public List<Plate> GetPlates() //Todo make call to Microservice
        {
            return new List<Plate>()
            {
                new Plate() {
                    Id = Guid.Parse("0812851E-3EC3-4D12-BAF6-C9F0E6DC2F76"),
                    Registration = "T44GUE",
                    PurchasePrice = 2722.51m,
                    SalePrice = 8995.00m,
                    Numbers = 44,
                    Letters = "TAG"
            }, new Plate() {
                    Id = Guid.Parse("DF81D7FC-319B-46A8-AB66-2574B4169C3D"),
                    Registration = "M44BEY",
                    PurchasePrice = 859.10m,
                    SalePrice = 8995.00m,
                    Numbers = 44,
                    Letters = "MAB"
            }, new Plate() {
                    Id = Guid.Parse("0E9C83BF-94E2-484A-97CB-A8B06E3410FD"),
                    Registration = "P777PER",
                    PurchasePrice = 1494.08m,
                    SalePrice = 4995.00m,
                    Numbers = 777,
                    Letters = "PYP"
                }
            };
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