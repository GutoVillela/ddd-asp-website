using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface IProductService
    {
        Task<ICommandResult> CreateProductAsync(CreateProductCommand command);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<ICommandResult> UpdateProductAsync(UpdateProductCommand command);
        Task<ICommandResult> DeleteProductAsync(DeleteProductCommand command);
    }
}
