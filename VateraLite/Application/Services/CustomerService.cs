using AutoMapper;
using VateraLite.Application.Dtos;
using VateraLite.Application.Exceptions;
using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;

namespace VateraLite.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetAsync(CancellationToken token = default)
        {
            var customers = await _unitOfWork.Customers.GetAsync(token: token);

            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto?> FindByIdAsync(int id, CancellationToken token = default)
        {
            var customer = await _unitOfWork.Customers.FindByIdAsync(id, token);

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> CreateAsync(CustomerDto customerDto, CancellationToken token = default)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _unitOfWork.Customers.AddAsync(customer, token);
            await _unitOfWork.CompleteAsync();
            customerDto.Id = customer.Id;

            return customerDto;
        }

        public async Task<CustomerDto> UpdateAsync(CustomerDto customerDto, CancellationToken token = default)
        {
            var customerInDb =
                await _unitOfWork.Customers.FindByIdAsync(customerDto.Id, token) ?? throw new ItemNotFoundException<int>(customerDto.Id);
            _mapper.Map(customerDto, customerInDb);
            await _unitOfWork.CompleteAsync();

            return customerDto;
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            var customerToRemove = await _unitOfWork.Customers.FindByIdAsync(id, token);
            if (customerToRemove != null)
            {
                _unitOfWork.Customers.Remove(customerToRemove);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
