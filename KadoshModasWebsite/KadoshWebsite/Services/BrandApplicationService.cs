using KadoshDomain.Commands;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Repositories;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class BrandApplicationService : IBrandApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandRepository _brandRepository;

        public BrandApplicationService(IUnitOfWork unitOfWork, IBrandRepository brandRepository)
        {
            _unitOfWork = unitOfWork;
            _brandRepository = brandRepository;
        }

        public async Task<ICommandResult> CreateBrandAsync(BrandViewModel brand)
        {
            CreateBrandCommand command = new();
            command.Name = brand.Name;

            BrandHandler brandHandler = new(_unitOfWork, _brandRepository);
            return await brandHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteBrandAsync(int id)
        {
            DeleteBrandCommand command = new();
            command.Id = id;

            BrandHandler brandHandler = new(_unitOfWork, _brandRepository);
            return await brandHandler.HandleAsync(command);
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

            BrandHandler brandHandler = new(_unitOfWork, _brandRepository);
            return await brandHandler.HandleAsync(command);
        }
    }
}
