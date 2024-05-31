namespace Ecome2.Models
{
    public class MessageVM
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Email { get; set; } // Kullanıcının e-posta adresi
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
        public bool IsRead { get; set; }
    }
}
