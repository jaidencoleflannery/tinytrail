using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models.LinkModel;
using Data.LinkContext;
using Services.LinkService;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Controllers.LinkController;

[ApiController]
[Route("/api/[controller]")]

[Authorize]
public class LinkController : ControllerBase
{

    private readonly LinkContext _db;
    private readonly ILinkService _link;
    private readonly ILogger<LinkController> _logger;

    public LinkController(LinkContext db, ILinkService link, ILogger<LinkController> logger)
    {
        _db = db;
        _link = link;
        _logger = logger;
    }

    [HttpGet("GetLink")]
    public async Task<IActionResult> GetLink(string url)
    {
        try
        {
            var val = await _link.GetLink(url, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(val);
        }
        catch (Exception ex)
        {
            _logger.LogError("(LinkController GetLink) Exception Encountered: {error}", ex);
            string response = ("Could Not Find URL: " + url);
            return BadRequest(response);
        }
    }

    [HttpGet("GetUsersLinks")]
    public async Task<IActionResult> GetUsersLinks()
    {
        try
        {
            var val = await _link.GetUsersLinks(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(val);
        }
        catch (Exception ex)
        {
            _logger.LogError("(LinkController GetLink) Exception Encountered: {error}", ex);
            string response = ("Could Not Find links for user");
            return BadRequest(response);
        }
    }

    [HttpGet("GetShortLink")]
    public async Task<IActionResult> GetShortLink(string url, string? optional)
    {
        try
        {
            var val = await _link.GetShortLink(url, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(val);
        }
        catch (Exception ex)
        {
            _logger.LogError("(LinkController GetLink) Exception Encountered: {error}", ex);
            string response = ("Could Not Find URL: " + url);
            return BadRequest(response);
        }
    }

    [HttpPost("TransformLink")]
    public async Task<IActionResult> PostLink(string url)
    {
        try
        {
            Link link = await _link.SetLink(url, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return CreatedAtAction("GetLink", link);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "(LinkController TransformLink) Exception Encountered: {Error}", ex.Message);
            return BadRequest("Duplicate Value - Please enter a unique URL.");
        }
    }
}