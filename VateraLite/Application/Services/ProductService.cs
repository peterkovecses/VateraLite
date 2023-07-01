using AutoMapper;
using VateraLite.Application.Dtos;
using VateraLite.Application.Exceptions;
using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;

namespace VateraLite.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAsync(CancellationToken token = default)
        {
            var products = await _unitOfWork.Products.GetAsync(token: token);

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetAsync(int quantity, CancellationToken token = default)
        {
            var products = await _unitOfWork.Products.GetAsync(take: quantity, token: token);

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> FindByIdAsync(Guid id, CancellationToken token = default)
        {
            var product = await _unitOfWork.Products.FindByIdAsync(id, token);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto, CancellationToken token = default)
        {
            productDto.Id = Guid.NewGuid();
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product, token);
            await _unitOfWork.CompleteAsync();
            productDto.Id = product.Id;

            return productDto;
        }

        public async Task<ProductDto> UpdateAsync(ProductDto productDto, CancellationToken token = default)
        {
            var productInDb =
                await _unitOfWork.Products.FindByIdAsync(productDto.Id, token) ?? throw new ItemNotFoundException<Guid>(productDto.Id);
            _mapper.Map(productDto, productInDb);
            await _unitOfWork.CompleteAsync();

            return productDto;
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            var productToRemove = await _unitOfWork.Products.FindByIdAsync(id, token);
            if (productToRemove != null)
            {
                _unitOfWork.Products.Remove(productToRemove);
                await _unitOfWork.CompleteAsync();
            }            
        }
    }
}
