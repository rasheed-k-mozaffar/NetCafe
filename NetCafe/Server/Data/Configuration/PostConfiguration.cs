using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar");

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(2_500_000);

        builder.HasMany(p => p.Comments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Tags)
                .WithMany(p => p.Posts);
    }
}
