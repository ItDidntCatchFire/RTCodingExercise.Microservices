using IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class PlateController : Controller
    {
        private readonly IRequestClient<CreatePlate> _createEndpoint;

        public PlateController(IRequestClient<CreatePlate> createEndpoint)
        {
            _createEndpoint = createEndpoint;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Plate plate)
        {
            if (plate.PurchasePrice == 0)
            {
                TempData["error"] = "Error - Failed to create please enter a valid price";
                return View("Index");
            }

            var response = await _createEndpoint.GetResponse<Plate>(new CreatePlate()
            {
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
            });

            var message = response.Message;

            if (plate == default)
            {
                TempData["error"] = "Error - Failed to create please try again";
                return View("Index");
            }

            TempData["success"] = "New plate added";
            return View("Index");
        }
    }
}
