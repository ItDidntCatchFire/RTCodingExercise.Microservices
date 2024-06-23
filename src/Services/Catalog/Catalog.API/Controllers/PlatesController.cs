using Catalog.API.Consumers;
using System.Linq.Expressions;
using System.Net;

namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlatesController : Controller
{
    private const int NUMBER_OF_PLATES = 20;

    private ApplicationDbContext _dbContext;
    private readonly ILogger<CreateConsumer> _logger;

    public PlatesController(ApplicationDbContext context, ILogger<PlatesController> logger)
    {
        _dbContext = context;
    }

    [HttpGet("GetPlates")]
    public IActionResult GetPlates(int page = 0, string orderBy = "id", bool orderAscending = true, int age = -1, string? initials = null, string discountCode = "")
    {
        var plates =
            (orderAscending ? _dbContext.Plates.OrderBy(SortBy(orderBy)) : _dbContext.Plates.OrderByDescending(SortBy(orderBy)))
            .ThenBy(x => x.Id)
            .Where(FilterBy(age, initials))
            .Where(x => x.Status != PlateStatus.Reserved)
            .Skip(page * NUMBER_OF_PLATES)
            .Take(NUMBER_OF_PLATES)
            .Select(x => new
            {
                Registration = x.Registration,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.CalculateSalesPrice(discountCode),
                Status = x.Status,
            });

        return Ok(plates);
    }

    [HttpPatch("ReservePlate")]
    public async Task<IActionResult> ReservePlate(Guid plateId)
    {
        var plate = _dbContext.Plates.FirstOrDefault(x => x.Id == plateId);
        if (plate == default)
            return BadRequest("Plate doesn't exist");

        if (plate.Status == PlateStatus.Reserved)
            return BadRequest("Plate is already reserved");

        if (plate.Status == PlateStatus.Sold)
            return BadRequest("Plate is already sold");

        plate.Status = PlateStatus.Reserved;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, "Failed to reserve plate");
            return BadRequest("Failed to update record");
        }

        return Ok();
    }

    [HttpPatch("PurchasePlate")]
    public async Task<IActionResult> PurchasePlate(Guid plateId, string discountCode = "")
    {
        var plate = _dbContext.Plates.FirstOrDefault(x => x.Id == plateId);
        if (plate == default)
            return BadRequest("Plate doesn't exist");

        if (plate.Status == PlateStatus.Sold)
            return BadRequest("Plate is already sold");

        var salePrice = plate.CalculateSalesPrice(discountCode);
        if (plate.IsPriceTooLow(salePrice))
            return BadRequest($"Code {discountCode} cannot be applied to this item");

        plate.Status = PlateStatus.Sold;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sell plate");
            return BadRequest("Failed to update record");
        }

        return Ok();
    }

    [HttpPost("CreatePlate")]
    public async Task<IActionResult> CreatePlate(Plate plate)
    {
        //validate Plate
        if (string.IsNullOrWhiteSpace(plate.Registration))
        {
            return BadRequest("Registration is invalid");
        }

        if (plate.PurchasePrice <= 0)
        {
            return BadRequest("Please supply a purchase price");
        }

        plate.Numbers = int.Parse(string.Concat(plate.Registration.Where(Char.IsDigit)));
        plate.Letters = string.Concat(plate.Registration.Where(x => !Char.IsDigit(x)));

        //if valid
        _dbContext.Plates.Add(plate);
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create plate");
            return BadRequest("Failed to create plate");
        }

        return StatusCode(StatusCodes.Status201Created, plate);
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

        if (age > -1)
            return (x => x.Numbers == age);

        if (initials != null && !string.IsNullOrEmpty(initials.Trim()))
            return (x => x.Letters.ToLower() == initials.Trim().ToLower());

        return (x => true);
    }
}