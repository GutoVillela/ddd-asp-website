using KadoshDomain.Commands.BrandCommands.CreateBrand;
using KadoshDomain.Commands.BrandCommands.DeleteBrand;
using KadoshDomain.Commands.BrandCommands.UpdateBrand;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Handlers;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class BrandApplicationService : IBrandApplicationService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IHandler<CreateBrandCommand> _createBrandHandler;
        private readonly IHandler<DeleteBrandCommand> _deleteBrandHandler;
        private readonly IHandler<UpdateBrandCommand> _updateBrandHandler;

        public BrandApplicationService(
            IBrandRepository brandRepository,
            IHandler<CreateBrandCommand> createBrandHandler,
            IHandler<DeleteBrandCommand> deleteBrandHandler,
            IHandler<UpdateBrandCommand> updateBrandHandler)
        {
            _brandRepository = brandRepository;
            _createBrandHandler = createBrandHandler;
            _deleteBrandHandler = deleteBrandHandler;
            _updateBrandHandler = updateBrandHandler;
        }

        public async Task<ICommandResult> CreateBrandAsync(BrandViewModel brand)
        {
            CreateBrandCommand command = new();
            command.Name = brand.Name;

            return await _createBrandHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteBrandAsync(int id)
        {
            DeleteBrandCommand command = new();
            command.Id = id;

            return await _deleteBrandHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<BrandViewModel>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.ReadAllAsync();
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
            var brand = await _brandRepository.ReadAsync(id);
            
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

            return await _updateBrandHandler.HandleAsync(command);
        }
    }
}
