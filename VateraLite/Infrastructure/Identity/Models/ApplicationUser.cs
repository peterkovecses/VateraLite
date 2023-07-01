using Microsoft.AspNetCore.Identity;
using VateraLite.Domain.Entities;

namespace VateraLite.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
