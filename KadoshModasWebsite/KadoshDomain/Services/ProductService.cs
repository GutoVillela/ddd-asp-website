using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;

namespace KadoshDomain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ICommandResult> CreateProductAsync(CreateProductCommand command)
        {
            ProductHandler productHandler = new(_productRepository);
            return await productHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteProductAsync(DeleteProductCommand command)
        {
            ProductHandler productHandler = new(_productRepository);
            return await productHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.ReadAllAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _productRepository.ReadAsync(id);
        }

        public async Task<ICommandResult> UpdateProductAsync(UpdateProductCommand command)
        {
            ProductHandler productHandler = new(_productRepository);
            return await productHandler.HandleAsync(command);
        }
    }
}
