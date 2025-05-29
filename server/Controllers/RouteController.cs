using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models.LinkModel;
using Data.LinkContext;
using Services.LinkService;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Controllers.RouteController;

[ApiController]
[Route("/api/[controller]")]

[AllowAnonymous] 
public class RouteController : ControllerBase
{
    private readonly LinkContext _db;
    private readonly ILinkService _link;
    private readonly ILogger<RouteController> _logger;

    public RouteController(LinkContext db, ILinkService link, ILogger<RouteController> logger)
    {
        _db = db;
        _link = link;
        _logger = logger;
    }

    [HttpGet("{url}")]
    public async Task<IActionResult> GoToLink([FromRoute] string url)
    {
        try
        {
            var val = await _link.GetLink(url, null);
            if (val == null) return NotFound();

            string redirectUrl = "";

            if (!val.Url.StartsWith("http://") && !val.Url.StartsWith("https://"))
            {
                redirectUrl = "http://" + val.Url;
            }
            else redirectUrl = val.Url;
            return Redirect(redirectUrl);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"(LinkController GetLink) Exception Encountered: {ex}");
            return BadRequest($"Could Not Find URL: {url}");
        }
    }
}


