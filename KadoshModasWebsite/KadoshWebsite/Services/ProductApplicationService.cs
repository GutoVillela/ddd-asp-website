using KadoshDomain.Commands.ProductCommands.CreateProduct;
using KadoshDomain.Commands.ProductCommands.DeleteProduct;
using KadoshDomain.Commands.ProductCommands.UpdateProduct;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshDomain.Queries.ProductQueries.GetAllProducts;
using KadoshDomain.Queries.ProductQueries.GetProductById;
using KadoshShared.Commands;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly ICommandHandler<CreateProductCommand> _createProductHandler;
        private readonly ICommandHandler<DeleteProductCommand> _deleteProductHandler;
        private readonly ICommandHandler<UpdateProductCommand> _updateProductHandler;
        private readonly IQueryHandler<GetAllProductsQuery, GetAllProductsQueryResult> _getAllProductsQueryHandler;
        private readonly IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult> _getProductByIdQueryHandler;

        public ProductApplicationService(
            ICommandHandler<CreateProductCommand> createProductHandler,
            ICommandHandler<DeleteProductCommand> deleteProductHandler,
            ICommandHandler<UpdateProductCommand> updateProductHandler,
            IQueryHandler<GetAllProductsQuery, GetAllProductsQueryResult> getAllProductsQueryHandler,
            IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult> getProductByIdQueryHandler
            )
        {
            _createProductHandler = createProductHandler;
            _deleteProductHandler = deleteProductHandler;
            _updateProductHandler = updateProductHandler;
            _getAllProductsQueryHandler = getAllProductsQueryHandler;
            _getProductByIdQueryHandler = getProductByIdQueryHandler;
        }

        public async Task<ICommandResult> CreateProductAsync(ProductViewModel Product)
        {
            CreateProductCommand command = new();
            command.Name = Product.Name;
            command.BarCode = Product.BarCode;
            command.Price = Product.Price;
            command.CategoryId = Product.CategoryId;
            command.BrandId = Product.BrandId;

            return await _createProductHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteProductAsync(int id)
        {
            DeleteProductCommand command = new();
            command.Id = id;

            return await _deleteProductHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var result = await _getAllProductsQueryHandler.HandleAsync(new GetAllProductsQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<ProductViewModel> productsViewModel = new();

            foreach(var product in result.Products)
            {
                productsViewModel.Add(GetViewModelFromDTO(product));
            }

            return productsViewModel;
        }

        public async Task<PaginatedListViewModel<ProductViewModel>> GetAllProductsPaginatedAsync(int currentPage, int pageSize)
        {
            GetAllProductsQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllProductsQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<ProductViewModel> productsViewModel = new();

            foreach (var product in result.Products)
            {
                productsViewModel.Add(GetViewModelFromDTO(product));
            }

            PaginatedListViewModel<ProductViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.ProductsCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.ProductsCount, pageSize);
            paginatedList.Items = productsViewModel;

            return paginatedList;
        }

        public async Task<ProductViewModel> GetProductAsync(int id)
        {

            GetProductByIdQuery query = new();
            query.ProductId = id;

            var result = await _getProductByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            ProductViewModel ProductViewModel = GetViewModelFromDTO(result.Product!);

            return ProductViewModel;
        }

        public async Task<ICommandResult> UpdateProductAsync(ProductViewModel Product)
        {
            UpdateProductCommand command = new();
            command.Id = Product.Id;
            command.Name = Product.Name;
            command.BarCode = Product.BarCode;
            command.Price = Product.Price;
            command.CategoryId = Product.CategoryId;
            command.BrandId = Product.BrandId;

            return await _updateProductHandler.HandleAsync(command);
        }

        private ProductViewModel GetViewModelFromDTO(ProductDTO product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                BarCode = product.BarCode,
                Price = product.Price,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            };
        }
    }
}
