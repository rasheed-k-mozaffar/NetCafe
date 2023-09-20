using Microsoft.AspNetCore.Mvc;

namespace NetCafe.Server.Controllers;
// this will provide the ApiController behvaior, to all inheriting controller classes.
[ApiController]
[Route("/api/[controller]")]
public class BaseController : ControllerBase
{
    public readonly ApplicationDbContext context;

    public BaseController(ApplicationDbContext context)
    {
        this.context = context;
    }
}
