using VateraLite.Infrastructure.Identity.Models;

namespace VateraLite.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = default!;
        public virtual IEnumerable<Order> Orders { get; set; } = default!;
    }
}