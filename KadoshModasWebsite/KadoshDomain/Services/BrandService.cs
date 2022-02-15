using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshShared.Repositories;

namespace KadoshDomain.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandRepository _brandRepository;

        public BrandService(IUnitOfWork unitOfWork, IBrandRepository brandRepository)
        {
            _unitOfWork = unitOfWork;
            _brandRepository = brandRepository;
        }

        public async Task<ICommandResult> CreateBrandAsync(CreateBrandCommand command)
        {
            BrandHandler brandHandler = new(_unitOfWork, _brandRepository);
            return await brandHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteBrandAsync(DeleteBrandCommand command)
        {
            BrandHandler BrandHandler = new(_unitOfWork, _brandRepository);
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
            BrandHandler brandHandler = new(_unitOfWork, _brandRepository);
            return await brandHandler.HandleAsync(command);
        }
    }
}
