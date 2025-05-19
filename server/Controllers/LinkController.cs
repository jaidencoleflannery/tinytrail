using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models.LinkModel;
using Data.LinkContext;
using Services.LinkService;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Controllers.LinkController;

[ApiController]
[Route("/api/[controller]")]
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

    [Authorize]
    [HttpGet("GetLink")]
    public async Task<IActionResult> GetLink(string url)
    {
        try
        {
            var val = await _link.GetLink(url, null);
            return Ok(val);
        }
        catch (Exception ex)
        {
            _logger.LogError("(LinkController GetLink) Exception Encountered: {error}", ex);
            string response = ("Could Not Find URL: " + url);
            return BadRequest(response);
        }
    }

    [Authorize]
    [HttpPost("TransformLink")]
    public async Task<IActionResult> PostLink(string url)
    {
        try
        {
            Link link = await _link.SetLink(url, null);
            return CreatedAtAction("GetLink", link);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "(LinkController TransformLink) Exception Encountered: {Error}", ex.Message);
            return BadRequest("Duplicate Value - Please enter a unique URL.");
        }
    }
}