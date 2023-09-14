using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCafe.Server.Data.Configuration;

namespace NetCafe.Server.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<NewsLetterSub> NewsLetterSubs { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new SeriesConfiguration());
        modelBuilder.ApplyConfiguration(new NewsLetterSubConfiguration());
    }
}
