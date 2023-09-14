using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar");

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(15_000);

        builder.HasMany(p => p.Comments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);

        builder.HasMany(p => p.Tags)
                .WithMany(p => p.Posts);
    }
}
