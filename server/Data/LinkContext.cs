using Models.LinkModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.LinkContext;

public class LinkContext : IdentityDbContext {

    public DbSet<Link> Links { get; set; }

    public LinkContext(DbContextOptions<LinkContext> options)
        : base(options) {}
}