namespace Catalog.API.Controllers;

public class PlatesController : Controller
{
    public ApplicationDbContext dbContext;

    public PlatesController(ApplicationDbContext _context)
    {
        dbContext = _context ;
    }

    public IActionResult GetPlates()
    {
        throw new NotImplementedException();
    }
}