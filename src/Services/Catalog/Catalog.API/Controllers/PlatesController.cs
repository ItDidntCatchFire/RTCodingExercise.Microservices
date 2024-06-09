namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlatesController : Controller
{
	private const int NUMBER_OF_PLATES = 20;

	public ApplicationDbContext dbContext;

    public PlatesController(ApplicationDbContext _context)
    {
        dbContext = _context ;
    }

    [HttpGet("GetPlates")]
    public IActionResult GetPlates(int page = 0)
    {
        var plates = dbContext.Plates
			.OrderBy(x => x.Id)
			.Skip(page * NUMBER_OF_PLATES)
			.Take(NUMBER_OF_PLATES)
            .Select(x => new
            {
                Registration = x.Registration,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.CalculateSalesPrice()
            });

        return Ok(plates);
    }
}