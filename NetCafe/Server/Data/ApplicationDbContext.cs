using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCafe.Server.Data.Configuration;

namespace NetCafe.Server.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    private readonly IConfiguration configuration;
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<NewsLetterSub> NewsLetterSubs { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new SeriesConfiguration());
        modelBuilder.ApplyConfiguration(new NewsLetterSubConfiguration());

        // seeding roles to the database
        modelBuilder.Entity<IdentityRole>().HasData
        (
            new IdentityRole { Id = configuration["Roles:AuthorRID"]!, Name = "AUTHOR", NormalizedName = "AUTHOR" },
            new IdentityRole { Id = configuration["Roles:UserRID"]!, Name = "USER", NormalizedName = "USER" }
        );

        // this hasher instance will be used to hash the author's password
        var hasher = new PasswordHasher<AppUser>();
        modelBuilder.Entity<AppUser>().HasData
        (
          new AppUser
          {
              Id = configuration["Author:ID"]!,
              UserName = configuration["Author:Email"],
              FullName = configuration["Author:FullName"],
              Email = configuration["Author:Email"],
              NormalizedEmail = configuration["Author:Email"]!.ToUpper(),
              PasswordHash = hasher.HashPassword(null!, $"{configuration["Author:Password"]}"),
              SecurityStamp = null,
          }
        );

        // seeding the relation and assignment of the author role to the main account
        modelBuilder.Entity<IdentityUserRole<string>>().HasData
        (
          new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
          {
              RoleId = configuration["Roles:AuthorRID"]!,
              UserId = configuration["Author:ID"]!
          }
        );
    }
}
