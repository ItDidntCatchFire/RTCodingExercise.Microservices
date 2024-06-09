using System.Linq.Expressions;

namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlatesController : Controller
{
	private const int NUMBER_OF_PLATES = 20;

	public ApplicationDbContext dbContext;

	public PlatesController(ApplicationDbContext _context)
	{
		dbContext = _context;
	}

	[HttpGet("GetPlates")]
	public IActionResult GetPlates(int page = 0, string orderBy = "id", bool orderAscending = true)
	{
		var plates = 
			(orderAscending ? dbContext.Plates.OrderBy(SortBy(orderBy)) : dbContext.Plates.OrderByDescending(SortBy(orderBy)))
			.ThenBy(x => x.Id)
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


	private Expression<Func<Plate, object>> SortBy(string property)
	{
		return property.ToLower() switch
		{
			"registration" => (x => x.Registration),
			"purchaseprice" => (x => x.PurchasePrice),
			"saleprice" => (x => x.SalePrice),
			_ => (x => x.Id)
		};
	}
}