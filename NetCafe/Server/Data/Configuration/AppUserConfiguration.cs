using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
       public void Configure(EntityTypeBuilder<AppUser> builder)
       {
              builder.HasKey(x => x.Id);

              builder.Property(p => p.FullName)
                     .IsRequired()
                     .HasMaxLength(150);

              builder.HasMany(p => p.Comments)
                     .WithOne(p => p.User)
                     .HasForeignKey(p => p.AppUserId);
       }
}
