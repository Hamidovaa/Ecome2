using System.ComponentModel.DataAnnotations;

namespace Ecome2.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime DateSent { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
        public string Category { get; set; }
    }
}
