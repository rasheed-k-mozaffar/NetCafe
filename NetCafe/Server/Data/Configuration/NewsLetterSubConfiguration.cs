using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class NewsLetterSubConfiguration : IEntityTypeConfiguration<NewsLetterSub>
{
    public void Configure(EntityTypeBuilder<NewsLetterSub> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Email)
               .IsRequired()
               .HasMaxLength(150)
               .HasAnnotation("RegularExpression", @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
    }
}
