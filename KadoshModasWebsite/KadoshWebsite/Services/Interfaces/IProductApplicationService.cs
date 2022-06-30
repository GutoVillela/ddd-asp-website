using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ICommandResult> CreateProductAsync(ProductViewModel product);
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<PaginatedListViewModel<ProductViewModel>> GetAllProductsPaginatedAsync(int currentPage, int pageSize);
        Task<PaginatedListViewModel<ProductViewModel>> GetProductsByNamePaginatedAsync(string productName, int currentPage, int pageSize);
        Task<ProductViewModel> GetProductAsync(int id);
        Task<ProductViewModel> GetProductByBarCodeAsync(string barCode);
        Task<ICommandResult> UpdateProductAsync(ProductViewModel product);
        Task<ICommandResult> DeleteProductAsync(int id);
    }
}
