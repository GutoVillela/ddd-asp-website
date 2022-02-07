using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshShared.Commands;

namespace KadoshDomain.Services.Interfaces
{
    public interface IBrandService
    {
        Task<ICommandResult> CreateBrandAsync(CreateBrandCommand command);
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandAsync(int id);
        Task<ICommandResult> UpdateBrandAsync(UpdateBrandCommand command);
        Task<ICommandResult> DeleteBrandAsync(DeleteBrandCommand command);
    }
}
