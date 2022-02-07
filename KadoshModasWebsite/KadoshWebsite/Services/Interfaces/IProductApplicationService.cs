using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ICommandResult> CreateProductAsync(ProductViewModel product);
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ProductViewModel> GetProductAsync(int id);
        Task<ICommandResult> UpdateProductAsync(ProductViewModel product);
        Task<ICommandResult> DeleteProductAsync(int id);
    }
}
