using Data.LinkContext;
using Microsoft.EntityFrameworkCore;
using Models.LinkModel;

namespace Services.LinkService;

public class LinkService : ILinkService
{
    private readonly LinkContext _db;
    private readonly ILogger<LinkService> _logger;
    public LinkService(LinkContext db, ILogger<LinkService> logger)
    {
        _db = db;
        _logger = logger;
    }
    // GetLink takes a short url, and returns a long url.
    public async Task<LinkDto> GetLink(string url, string? userId)
    {
        try
        {
            var entity = await _db.Links.FirstOrDefaultAsync(val => val.ShortUrl == url);
            if (entity != null)
            {
                LinkDto link = new LinkDto { Url = entity.Url, ShortUrl = "http://localhost:5137/api/Route/" + entity.ShortUrl }; // REPLACE THIS WITH THE PROD URL ON DEPLOY
                _logger.LogInformation($"(GetLink) URL GRABBED: {entity.Url}");
                return link;
            }
            else throw new KeyNotFoundException("(GetLink) Link not found.");
        }
        catch (Exception ex)
        {
            throw new Exception("(LinkService GetLink) Exception Encountered: {Error}", ex);
        }
    }
    
    public async Task<LinkDto> GetShortLink(string url, string? userId)
    {
        try
        {
            var entity = await _db.Links.FirstOrDefaultAsync(val => val.ShortUrl == url);
            if (entity != null)
            {
                LinkDto link = new LinkDto { Url = entity.Url, ShortUrl = "http://localhost:5137/api/Route/" + entity.ShortUrl }; // REPLACE THIS WITH THE PROD URL ON DEPLOY
                _logger.LogInformation($"(GetLink) URL GRABBED: {entity.Url}");
                return link;
            }
            else throw new KeyNotFoundException("(GetLink) Link not found.");
        }
        catch (Exception ex)
        {
            throw new Exception("(LinkService GetLink) Exception Encountered: {Error}", ex);
        }
    }

    public async Task<Link> SetLink(string url, string userId)
    {
        if (await _db.Links.FirstOrDefaultAsync(val => val.Url == url) != null)
        {
            throw new Exception("(LinkService SetLink) Duplicate Value");
        }
        else
        {
            try
            {
                Link link = new Link { Url = url, UserId = userId, ShortUrl = this.GenerateShortLink() };
                _db.Links.Add(link);
                await _db.SaveChangesAsync();
                Link result = new Link { Id = link.Id, Url = url, ShortUrl = link.ShortUrl, UserId = userId };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("(LinkService SetLink) Exception Encountered: {Error}", ex);
            }
        }
    }

    private string GenerateShortLink() {
        var shortUrl = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            shortUrl = shortUrl
                        .Replace("=", "")
                        .Replace("+", "")
                        .Replace("/", "");
        _logger.LogInformation("SHORTENED URL: {url}", shortUrl);
        return shortUrl;
    }
}