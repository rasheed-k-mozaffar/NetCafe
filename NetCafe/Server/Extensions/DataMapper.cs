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
            CoverImageUrl = post.CoverImageUrl,
            Title = post.Title,
            PublishedOn = post.PublishedOn,
            ModifiedOn = post.ModifiedOn,
            Likes = post.Likes,
            IsPublished = post.IsPublished,
            Tags = post.Tags?.Select(t => t.ToTagSummary()).ToList()
        };
    }

    public static PostDto ToPost(this Post post)
    {
        return new PostDto
        {
            PostId = post.Id,
            SeriesId = post.SeriesId,
            Series = post.Series?.ToSeriesSummary(),
            CoverImageUrl = post.CoverImageUrl,
            Title = post.Title,
            Content = post.Content,
            Views = post.Views,
            Likes = post.Likes,
            IsPublished = post.IsPublished,
            PublishedOn = post.PublishedOn,
            ModifiedOn = post.ModifiedOn,
            Comments = post.Comments?.Select(c => c.ToCommentDto()).ToList(),
            Tags = post.Tags?.Select(t => t.ToTagSummary()).ToList()
        };
    }
    #endregion

    #region Tag Mappers
    public static TagDto ToTagDto(this Tag tag)
    {
        return new TagDto
        {
            TagId = tag.Id,
            Name = tag.Name,
            Description = tag.Description
        };
    }

    public static TagSummaryDto ToTagSummary(this Tag tag)
    {
        return new TagSummaryDto
        {
            TagId = tag.Id,
            Name = tag.Name
        };
    }

    public static TagDataDto ToTagData(this Tag tag)
    {
        return new TagDataDto
        {
            TagId = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            Posts = tag.Posts?.Select(p => p.ToPostSummary()).ToList()
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
            ParentCommentId = comment.ParentCommentId,
            AppUserId = comment.AppUserId,
            Content = comment.Content,
            PostedOn = comment.PostedOn,
            Replies = comment.Replies?.Select(r => r.ToCommentDto()).ToList(),
            Likes = comment.Likes
        };
    }
    #endregion

    #region Series Mappers
    public static SeriesSummaryDto ToSeriesSummary(this Series series)
    {
        return new SeriesSummaryDto
        {
            SeriesId = series.Id,
            Name = series.Name,
            Description = series.Description,
            PostsCount = series.Posts?.Count,
            CoverImageUrl = series.CoverImageUrl,
        };
    }
    public static SeriesDto ToSeriesDto(this Series series)
    {
        return new SeriesDto
        {
            SeriesId = series.Id,
            Name = series.Name,
            Description = series.Description,
            CoverImageUrl = series.CoverImageUrl,
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
            PublishedOn = DateTime.UtcNow,
            CoverImageUrl = post.CoverImageUrl,
            IsPublished = post.IsPublished,
        };
    }
    #endregion

    #region Tag Mappers 
    public static Tag ToTag(this TagDto tag)
    {
        return new Tag
        {
            Id = tag.TagId,
            Name = tag.Name,
            Description = tag.Description
        };
    }
    public static Tag ToTagCreate(this TagCreateDto tag)
    {
        return new Tag
        {
            Name = tag.Name,
            Description = tag.Description
        };
    }
    #endregion

    #region Comment Mappers
    public static Comment ToCommentAdd(this CommentCreateDto comment)
    {
        return new Comment
        {
            Content = comment.Content,
            PostedOn = DateTime.UtcNow
        };
    }
    #endregion

    #region Series Mappers
    public static Series ToSeriesCreate(this SeriesCreateDto series)
    {
        return new Series
        {
            Name = series.Name,
            Description = series.Description,
            CoverImageUrl = series.CoverImageUrl
        };
    }
    #endregion
}
