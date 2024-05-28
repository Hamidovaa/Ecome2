using System.ComponentModel.DataAnnotations;

namespace Ecome2.Models
{
    public class ResetPassword
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

    }
}
