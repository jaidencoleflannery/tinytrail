using Models.LinkModel;
using Microsoft.EntityFrameworkCore;

namespace Data.LinkContext;

public class LinkContext : DbContext {

    public DbSet<Link> Links { get; set; }

    public LinkContext(DbContextOptions<LinkContext> options)
        : base(options) {}
}