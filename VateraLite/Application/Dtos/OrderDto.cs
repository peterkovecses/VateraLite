using VateraLite.Domain.Entities;

namespace VateraLite.Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public int CustomerId { get; set; }
    }
}
