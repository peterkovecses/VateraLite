namespace VateraLite.Application.Dtos
{
    public class OrderWithProductsDto
    {
        public OrderDto Order { get; set; } = default!;
        public IEnumerable<ProductDto> Products { get; set; } = default!;
    }
}
