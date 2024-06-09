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
	public IActionResult GetPlates(int page = 0, string orderBy = "id", bool orderAscending = true, int age = -1, string? initials = null)
	{
		var plates =
			(orderAscending ? dbContext.Plates.OrderBy(SortBy(orderBy)) : dbContext.Plates.OrderByDescending(SortBy(orderBy)))
			.ThenBy(x => x.Id)
			.Where(FilterBy(age, initials))
			.Skip(page * NUMBER_OF_PLATES)
			.Take(NUMBER_OF_PLATES)
			.Select(x => new
			{
				Registration = x.Registration,
				PurchasePrice = x.PurchasePrice,
				SalePrice = x.CalculateSalesPrice(),
				Reserved = x.IsReserved,
			});

		return Ok(plates);
	}

	[HttpPatch("ReservePlate")]
	public async Task<IActionResult> ReservePlate(Guid plateId)
	{
		var plate = dbContext.Plates.FirstOrDefault(x => x.Id == plateId);
		if (plate == default)
			return BadRequest("Plate doesn't exist");

		if (plate.IsReserved)
			return BadRequest("Plate is already reserved");

		plate.IsReserved = true;

		try
		{
			await dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			Log.Logger.Error(ex, "Failed to reserve plate");
			return BadRequest("Failed to update record");
		}

		return Ok();
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

	private Expression<Func<Plate, bool>> FilterBy(int age, string initials)
	{
		if (age > -1 && initials != null && !string.IsNullOrEmpty(initials.Trim()))
			return (x => x.Numbers == age && x.Letters.ToLower() == initials.Trim().ToLower());

		if (age > -1 )
			return (x => x.Numbers == age);

		if (initials != null && !string.IsNullOrEmpty(initials.Trim()))
			return (x => x.Letters.ToLower() == initials.Trim().ToLower());

		return (x => true);
	}
}