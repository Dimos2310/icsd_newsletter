using Microsoft.EntityFrameworkCore;
using NewsLetter.Models;
using NewsLetter.Models.Context;

namespace NewsLetter.Services;

public class CommentService
{
    private readonly DataContext _db = new();
    public string authorizationId { get; set; }

    public CommentService(string authorizationId)
    {
        this.authorizationId = authorizationId;
    }

    // public void CommentOnPost(int postId, Comment comment)
    // {
    //     // Find the Post that is about to be commented
    //     var post = _db.News
    //         .Include(sh => sh.Comments)
    //         .FirstOrDefault(sh => sh.Id == postId);

    //     if (post is null) throw new IOException("Invalid Post ID");

    //     // Add the appropriate types required for new Comment
    //     var toSave = CreateANewComment(comment);

    //     // Comment at the post
    //     var comments = post.Comments;
    //     comments.Add(toSave);

    //     // Save Changes
    //     _db.Posts.Update(post);
    //     _db.SaveChanges();
    // }

    // private Comment CreateANewComment(Comment comment)
    // {
    //     // Save current date of the comment 
    //     comment.CreationDate = DateTime.Now;

    //     // Set status of created
    //     comment.Status = CommentStatus.CREATED.ToString();

    //     _db.Comments.Add(comment);
    //     _db.SaveChanges();
    //     return comment;
    // }

    // public List<Comment> GetComments(string? status = null)
    // {
    //     // We are checking if the user gave a valid Status 
    //     if (status != null &&
    //         !Enum.IsDefined(typeof(CommentStatus), status.ToUpper())
    //        )
    //         throw new IOException("Invalid status");

    //     IQueryable<Comment> comments = _db.Comments;
    //     if (status != null) comments = comments.Where(sh => sh.Status.Equals(status.ToUpper()));
    //     return comments.ToList();
    // }

    // public void UpdateComment(Comment comment)
    // {
    //     var currentComment = _db.Comments.FirstOrDefault(sh => sh.Id == comment.Id);
    //     if (currentComment == null) return;
    //     currentComment.Content = comment.Content;
    //     currentComment.FullName = comment.FullName;
    //     currentComment.Status = comment.Status;
    //     _db.Comments.Update(currentComment);
    //     _db.SaveChanges();
    // }

    // public void SetCommentStatus(long id, string status)
    // {
    //     if (!Enum.IsDefined(typeof(CommentStatus), status.ToUpper()))
    //         throw new IOException("Invalid Comment Status");

    //     var currentComment = _db.Comments.FirstOrDefault(sh => sh.Id == id);
    //     if (currentComment is null) throw new IOException("Comment not found");
    //     if (string.Equals(status.ToUpper(), CommentStatus.ACCEPTED.ToString()))
    //     {
    //         currentComment.Status = CommentStatus.ACCEPTED.ToString();
    //         _db.Comments.Update(currentComment);
    //     }

    //     if (string.Equals(status.ToUpper(), CommentStatus.REJECTED.ToString()))
    //         _db.Remove(currentComment);

    //     _db.SaveChanges();
    // }

    // public void DeleteComment(Comment comment)
    // {
    //     var currenComment = _db.Comments.FirstOrDefault(sh => sh.Id == comment.Id);
    //     if (currenComment == null) return;
    //     _db.Comments.Remove(currenComment);
    //     _db.SaveChanges();
    // }

    
}