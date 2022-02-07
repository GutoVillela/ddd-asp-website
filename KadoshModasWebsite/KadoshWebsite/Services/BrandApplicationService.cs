using KadoshDomain.Commands;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class BrandApplicationService : IBrandApplicationService
    {
        private readonly IBrandService _brandService;

        public BrandApplicationService(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<ICommandResult> CreateBrandAsync(BrandViewModel brand)
        {
            CreateBrandCommand command = new();
            command.Name = brand.Name;

            return await _brandService.CreateBrandAsync(command);
        }

        public async Task<ICommandResult> DeleteBrandAsync(int id)
        {
            DeleteBrandCommand command = new();
            command.Id = id;

            return await _brandService.DeleteBrandAsync(command);
        }

        public async Task<IEnumerable<BrandViewModel>> GetAllBrandsAsync()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            List<BrandViewModel> brandsViewModel = new();

            foreach(var brand in brands)
            {
                brandsViewModel.Add(new BrandViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name
                });
            }

            return brandsViewModel;
        }

        public async Task<BrandViewModel> GetBrandAsync(int id)
        {
            var brand = await _brandService.GetBrandAsync(id);
            
            if(brand is null)
                throw new ApplicationException(BrandServiceMessages.ERROR_BRAND_ID_NOT_FOUND);

            BrandViewModel brandViewModel = new()
            {
                Id = brand.Id,
                Name = brand.Name
            };

            return brandViewModel;
        }

        public async Task<ICommandResult> UpdateBrandAsync(BrandViewModel brand)
        {
            UpdateBrandCommand command = new();
            command.Id = brand.Id;
            command.Name = brand.Name;

            return await _brandService.UpdateBrandAsync(command);
        }
    }
}
