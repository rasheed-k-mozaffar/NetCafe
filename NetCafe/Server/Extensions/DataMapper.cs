using NetCafe.Shared;

namespace NetCafe.Server.Extensions;

public static class DataMapper
{
    #region  Post Mappers
    public static PostSummaryDto ToPostSummary(this Post post)
    {
        return new PostSummaryDto
        {
            PostId = post.Id,
            CoverImage = post.CoverImage,
            Title = post.Title,
            PublishedOn = post.PublishedOn,
            Likes = post.Likes,
            Tags = post.Tags?.Select(t => t.ToTagDto()).ToList()
        };
    }

    public static PostDto ToPost(this Post post)
    {
        return new PostDto
        {
            PostId = post.Id,
            SeriesId = post.SeriesId,
            Series = post.Series?.ToSeriesDto(),
            CoverImage = post.CoverImage,
            Title = post.Title,
            Content = post.Content,
            Views = post.Views,
            Likes = post.Likes,
            PublishedOn = post.PublishedOn,
            Comments = post.Comments?.Select(c => c.ToCommentDto()).ToList(),
            Tags = post.Tags?.Select(t => t.ToTagDto()).ToList()
        };
    }
    #endregion

    #region Tag Mappers
    public static TagDto ToTagDto(this Tag tag)
    {
        return new TagDto
        {
            TagId = tag.Id,
            Name = tag.Name
        };
    }
    #endregion

    #region Comment Mappers
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto
        {
            CommentId = comment.Id,
            PostId = comment.PostId,
            AppUserId = comment.AppUserId,
            Content = comment.Content,
            PostedOn = comment.PostedOn,
            Replies = comment.Replies?.Select(r => r.ToCommentDto()).ToList(),
            Likes = comment.Likes
        };
    }
    #endregion

    #region Series Mappers
    public static SeriesDto ToSeriesDto(this Series series)
    {
        return new SeriesDto
        {
            SeriesId = series.Id,
            Name = series.Name,
            Description = series.Description,
            Posts = series.Posts?.Select(p => p.ToPostSummary()).ToList()
        };
    }
    #endregion
}

public static class ModelMapper
{
    #region Post Mappers
    public static Post ToPostCreate(this PostCreateDto post)
    {
        return new Post
        {
            // cover image handled in the controller
            Title = post.Title,
            Content = post.Content,
            SeriesId = post.SeriesId,
            Tags = post.Tags?.Select(t => t.ToTag()).ToList(),
            PublishedOn = DateTime.UtcNow
        };
    }

    public static Post ToPostUpdate(this PostUpdateDto post)
    {
        return new Post
        {
            // cover image handled in the controller
            Title = post.Title,
            Content = post.Content,
            SeriesId = post.SeriesId,
            Tags = post.Tags?.Select(t => t.ToTag()).ToList(),
            ModifiedOn = DateTime.UtcNow
        };
    }
    #endregion

    #region Tag Mappers 
    public static Tag ToTag(this TagDto tag)
    {
        return new Tag
        {
            Id = tag.TagId,
            Name = tag.Name
        };
    }
    #endregion
}
