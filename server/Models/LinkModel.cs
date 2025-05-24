using Models.UserModel;

namespace Models.LinkModel;

public class Link {
    public int Id { get; set; }
    // original url
    public string Url { get; set; }
    // transformed url
    public string ShortUrl { get; set; }
    public string UserId { get; set; }
    public User User { get; set; } = default!;
}

public class LinkDto {
    // original url
    public string Url { get; set; }
    // transformed url
    public string ShortUrl { get; set; }
    public string? UserId { get; set; }
}