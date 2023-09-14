using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewsLetter.Models
{
public class Topic
{
    [Key] public long Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    [AllowNull] public Topic ParentTopic { get; set; }
    // [AllowNull] public String Parent { get; set; }

    public string Status { get; set; }
    public User Owner { get; set; }
}
}