using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface IBrandApplicationService
    {
        Task<ICommandResult> CreateBrandAsync(BrandViewModel brand);
        Task<IEnumerable<BrandViewModel>> GetAllBrandsAsync();
        Task<BrandViewModel> GetBrandAsync(int id);
        Task<ICommandResult> UpdateBrandAsync(BrandViewModel brand);
        Task<ICommandResult> DeleteBrandAsync(int id);
    }
}
