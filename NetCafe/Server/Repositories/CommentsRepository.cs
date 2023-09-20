﻿
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NetCafe.Server.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly ApplicationDbContext context;
    private readonly UserIdentityOptions identityOptions;

    public CommentsRepository(ApplicationDbContext context, UserIdentityOptions identityOptions)
    {
        this.context = context;
        this.identityOptions = identityOptions;
    }

    public async Task<bool> AddCommentAsync(Guid postId, Comment comment)
    {
        // Assign the foreign keys
        comment.PostId = postId;
        comment.AppUserId = identityOptions.UserId;
        var result = await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();

        if (result.State == EntityState.Added)
        {
            return true;
        }
        else
        {
            throw new DataInsertionFailedException(message: "Something went wrong while attempting to add the comment.");
        }
    }

    public async Task<bool> AddReplyToCommentAsync(Guid commentId, Comment reply)
    {
        var parentComment = await context.Comments.FindAsync(commentId);

        // In case the comment being replied to was missing
        if (parentComment is null)
        {
            throw new NotFoundException(message: "No comment was found with the given ID.");
        }

        // map the relational properties
        reply.PostId = parentComment.PostId;
        reply.AppUserId = identityOptions.UserId;
        reply.ParentCommentId = parentComment.Id;

        var result = await context.Comments.AddAsync(reply);
        await context.SaveChangesAsync();

        if (result.State == EntityState.Added)
        {
            return true;
        }
        else
        {
            throw new DataInsertionFailedException(message: "Something went wrong while attempting to add the comment.");
        }
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId)
    {
        var comment = await context.Comments.FindAsync(commentId);

        if (comment is null)
        {
            throw new NotFoundException(message: "No comment was found with the given ID.");
        }

        var result = context.Comments.Remove(comment);
        await context.SaveChangesAsync();

        if (result.State == EntityState.Deleted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<Comment> GetCommentAsync(Guid commentId)
    {
        var comment = await context.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null)
        {
            throw new NotFoundException(message: "No comment was found with the given ID.");
        }

        return comment;
    }

    public async Task<ICollection<Comment>> GetCommentsAsync(Guid postId)
    {
        // get the comments without tracking them by the context
        var comments = await context.Comments
            .AsNoTracking().ToListAsync();

        return comments;
    }

    public Task<bool> UpdateCommentAsync(Guid commentId, Comment comment)
    {
        throw new NotImplementedException();
    }
}