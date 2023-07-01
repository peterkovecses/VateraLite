namespace VateraLite.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = default!;
        public virtual IEnumerable<Product> Products { get; set; } = default!;
    }
}
