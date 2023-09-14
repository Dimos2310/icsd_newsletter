using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewsLetter.Models
{
public class NewsMod
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
    public List<String> Comments { get; set; }
    public List<String> Topic { get; set; }
    public String Owner { get; set; }
}
}