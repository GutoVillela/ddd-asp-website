using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;

namespace KadoshDomain.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<ICommandResult> CreateBrandAsync(CreateBrandCommand command)
        {
            BrandHandler brandHandler = new(_brandRepository);
            return await brandHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteBrandAsync(DeleteBrandCommand command)
        {
            BrandHandler BrandHandler = new(_brandRepository);
            return await BrandHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.ReadAllAsync();
        }

        public async Task<Brand> GetBrandAsync(int id)
        {
            return await _brandRepository.ReadAsync(id);
        }

        public async Task<ICommandResult> UpdateBrandAsync(UpdateBrandCommand command)
        {
            BrandHandler brandHandler = new(_brandRepository);
            return await brandHandler.HandleAsync(command);
        }
    }
}
