using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
       public void Configure(EntityTypeBuilder<AppUser> builder)
       {
              builder.HasKey(x => x.Id);

              builder.Property(p => p.FirstName)
                     .IsRequired()
                     .HasMaxLength(75);

              builder.Property(p => p.LastName)
                     .IsRequired()
                     .HasMaxLength(75);

              builder.HasMany(p => p.Comments)
                     .WithOne(p => p.User)
                     .HasForeignKey(p => p.AppUserId);
       }
}
