namespace VateraLite.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
