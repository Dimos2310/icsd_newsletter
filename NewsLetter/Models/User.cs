using Microsoft.EntityFrameworkCore;

namespace NewsLetter.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string AuthToken { get; set; }
        public DateTime TokenExpire { get; set; }
        public string Status { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public ICollection<Topic> topics { get; set; }
    }



}