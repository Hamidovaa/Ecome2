using Microsoft.AspNetCore.Identity;

namespace Ecome2.Models
{
    public class ProgramUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public List<Order> Orders { get; set; }
    }
}
