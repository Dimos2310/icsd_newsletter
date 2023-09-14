using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewsLetter.Models
{
    public class Comment
    {
        [Key] public int Id { get; set; }
        [AllowNull] public string Owner { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
    }

}