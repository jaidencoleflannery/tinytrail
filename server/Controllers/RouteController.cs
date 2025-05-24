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
[Route("/")]

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
         _logger.LogInformation($"Searching for {url}");
        try
        {
            var val = await _link.GetShortLink(url, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _logger.LogInformation($"Redirecting to {val}");
            return RedirectPermanent(val.Url);
        }
        catch (Exception ex)
        {
            _logger.LogError("(LinkController GetLink) Exception Encountered: {error}", ex);
            string response = "Could Not Find URL: " + url;
            return BadRequest(response);
        }
    }
}


