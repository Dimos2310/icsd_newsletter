using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewsLetter.Models
{
public class News
{
    [Key] public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    [AllowNull] 
    public string Status { get; set; }
    [AllowNull] 
    public DateTime CreatationDate { get; set; }
    [AllowNull] 
    public DateTime PublicationDate { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Topic> Topic { get; set; }
    public User Owner { get; set; }
}
}