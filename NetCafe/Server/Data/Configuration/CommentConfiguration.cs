using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCafe.Server;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Content)
               .IsRequired()
               .HasMaxLength(1000);

        // builder.HasOne(p => p.ParentComment)
        //        .WithMany(p => p.Replies)
        //        .HasForeignKey(p => p.ParentCommentId)
        //        .OnDelete(DeleteBehavior.NoAction); // delete replies when comment is deleted
    }
}
