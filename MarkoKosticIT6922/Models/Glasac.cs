using Microsoft.AspNetCore.Identity;

namespace MarkoKosticIT6922.Models
{
    public class Glasac : IdentityUser
    {
        public string? displayName { get; set; }
        public DateTime joinedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Argument> Argumenti { get; set; } = new List<Argument>();
        public ICollection<Glasanje> Glasanja { get; set; } = new List<Glasanje>();
    }
}
