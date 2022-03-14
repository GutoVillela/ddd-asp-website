using KadoshDomain.Commands.BrandCommands.CreateBrand;
using KadoshDomain.Commands.BrandCommands.DeleteBrand;
using KadoshDomain.Commands.BrandCommands.UpdateBrand;
using KadoshDomain.Queries.BrandQueries.GetAllBrands;
using KadoshDomain.Queries.BrandQueries.GetBrandById;
using KadoshShared.Commands;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class BrandApplicationService : IBrandApplicationService
    {
        private readonly ICommandHandler<CreateBrandCommand> _createBrandHandler;
        private readonly ICommandHandler<DeleteBrandCommand> _deleteBrandHandler;
        private readonly ICommandHandler<UpdateBrandCommand> _updateBrandHandler;
        private readonly IQueryHandler<GetAllBrandsQuery, GetAllBrandsQueryResult> _getAllBrandsQueryHandler;
        private readonly IQueryHandler<GetBrandByIdQuery, GetBrandByIdQueryResult> _getBrandByIdQueryHandler;

        public BrandApplicationService(
            ICommandHandler<CreateBrandCommand> createBrandHandler,
            ICommandHandler<DeleteBrandCommand> deleteBrandHandler,
            ICommandHandler<UpdateBrandCommand> updateBrandHandler,
            IQueryHandler<GetAllBrandsQuery, GetAllBrandsQueryResult> getAllBrandsQueryHandler,
            IQueryHandler<GetBrandByIdQuery, GetBrandByIdQueryResult> getBrandByIdQueryHandler)
        {
            _createBrandHandler = createBrandHandler;
            _deleteBrandHandler = deleteBrandHandler;
            _updateBrandHandler = updateBrandHandler;
            _getAllBrandsQueryHandler = getAllBrandsQueryHandler;
            _getBrandByIdQueryHandler = getBrandByIdQueryHandler;
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
            var result = await _getAllBrandsQueryHandler.HandleAsync(new GetAllBrandsQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<BrandViewModel> brandsViewModel = new();

            foreach (var brand in result.Brands)
            {
                brandsViewModel.Add(new BrandViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name
                });
            }

            return brandsViewModel;
        }

        public async Task<PaginatedListViewModel<BrandViewModel>> GetAllBrandsPaginatedAsync(int currentPage, int pageSize)
        {
            GetAllBrandsQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllBrandsQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<BrandViewModel> brandsViewModel = new();

            foreach (var brand in result.Brands)
            {
                brandsViewModel.Add(new BrandViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name
                });
            }

            PaginatedListViewModel<BrandViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.BrandsCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.BrandsCount, pageSize);
            paginatedList.Items = brandsViewModel;



            return paginatedList;
        }

        public async Task<BrandViewModel> GetBrandAsync(int id)
        {
            GetBrandByIdQuery query = new();
            query.BrandId = id;

            var result = await _getBrandByIdQueryHandler.HandleAsync(query);
            
            if(!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            BrandViewModel brandViewModel = new()
            {
                Id = result.Brand!.Id,
                Name = result.Brand!.Name
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
