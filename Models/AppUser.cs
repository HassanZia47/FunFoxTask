using Microsoft.AspNetCore.Identity;

namespace FunFoxTask.Models
{
    public class AppUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
