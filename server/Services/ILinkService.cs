using Models.LinkModel;

namespace Services.LinkService;

public interface ILinkService {
    public Task<LinkDto> GetShortLink(string url, string? userId);
    public Task<LinkDto> GetLink(string url, string userId);
    public Task<Link> SetLink(string url, string userId);
}