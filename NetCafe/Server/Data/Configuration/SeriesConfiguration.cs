using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server.Data.Configuration;

public class SeriesConfiguration : IEntityTypeConfiguration<Series>
{
        public void Configure(EntityTypeBuilder<Series> builder)
        {
                builder.HasKey(x => x.Id);

                builder.Property(p => p.Name)
                        .IsRequired()
                        .HasMaxLength(100);

                builder.Property(p => p.Description)
                        .HasMaxLength(500);

                builder.HasMany(p => p.Posts)
                        .WithOne(p => p.Series)
                        .HasForeignKey(p => p.SeriesId);
        }
}
