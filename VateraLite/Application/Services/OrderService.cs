using AutoMapper;
using System.Linq.Expressions;
using VateraLite.Application.Dtos;
using VateraLite.Application.Exceptions;
using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;

namespace VateraLite.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAsync(CancellationToken token = default)
        {
            var orders = await _unitOfWork.Orders.GetAsync(token: token);

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto?> FindByIdAsync(Guid id, CancellationToken token = default)
        {
            var order = await _unitOfWork.Orders.FindByIdAsync(id, token);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken token = default)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _unitOfWork.Orders.AddAsync(order, token);
            await _unitOfWork.CompleteAsync();
            orderDto.Id = order.Id;

            return orderDto;
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, IEnumerable<ProductDto> productDtos, CancellationToken token = default)
        {
            var order = _mapper.Map<Order>(orderDto);            
            await _unitOfWork.Orders.AddAsync(order, token);
            foreach (var productDto in productDtos)
            {
                var productInDb =
                    await _unitOfWork.Products.FindByIdAsync(productDto.Id, token) ?? throw new ItemNotFoundException<Guid>(productDto.Id);
                _mapper.Map(productDto, productInDb);
            }
            await _unitOfWork.CompleteAsync();
            orderDto.Id = order.Id;

            return orderDto;
        }

        public async Task<OrderDto> UpdateAsync(OrderDto orderDto, CancellationToken token = default)
        {
            var orderInDb =
                await _unitOfWork.Orders.FindByIdAsync(orderDto.Id, token) ?? throw new ItemNotFoundException<Guid>(orderDto.Id);
            _mapper.Map(orderDto, orderInDb);
            await _unitOfWork.CompleteAsync();

            return orderDto;
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            var orderToRemove = await _unitOfWork.Orders.FindByIdAsync(id, token);
            if (orderToRemove != null)
            {
                _unitOfWork.Orders.Remove(orderToRemove);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<bool> CanOrder(int customerId, int quantity, CancellationToken token = default)
        {
            Expression<Func<Order, bool>> predicate = o => o.CustomerId == customerId;
            var customerOrders = await _unitOfWork.Orders.GetAsync(predicate: predicate, token: token);
            var pastOrderedProducts = customerOrders.SelectMany(o => o.Products);
            return pastOrderedProducts.Count() + quantity <= 10;
        }

        public bool ValidateAmount(int quantity)
        {
            return quantity <= 0 || quantity > 2;
        }
    }
}
