namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlatesController : Controller
{
    public ApplicationDbContext dbContext;

    public PlatesController(ApplicationDbContext _context)
    {
        dbContext = _context ;
    }

    [HttpGet("GetPlates")]
    public IActionResult GetPlates()
    {
        var plates = dbContext.Plates.Select(x => new
        {
            Registration = x.Registration,
            PurchasePrice = x.PurchasePrice,
            SalePrice = x.CalculateSalesPrice()
        });

        return Ok(plates);
    }






}