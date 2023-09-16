using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(50)
               .HasColumnType("varchar");

        builder.Property(p => p.Description)
               .IsRequired()
               .HasMaxLength(5000);

        builder.HasMany(p => p.Posts)
               .WithMany(p => p.Tags);
    }
}
