using Models.LinkModel;
using Models.UserModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.LinkContext;

public class LinkContext : IdentityDbContext {

    public DbSet<Link> Links { get; set; }
    public DbSet<User> Users { get; set; }

    public LinkContext(DbContextOptions<LinkContext> options)
        : base(options) {}
}