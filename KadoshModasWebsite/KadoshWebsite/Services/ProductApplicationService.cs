using KadoshDomain.Commands.ProductCommands.CreateProduct;
using KadoshDomain.Commands.ProductCommands.DeleteProduct;
using KadoshDomain.Commands.ProductCommands.UpdateProduct;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Handlers;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IProductRepository _productRepository;
        private readonly IHandler<CreateProductCommand> _createProductHandler;
        private readonly IHandler<DeleteProductCommand> _deleteProductHandler;
        private readonly IHandler<UpdateProductCommand> _updateProductHandler;

        public ProductApplicationService(
            IProductRepository productRepository,
            IHandler<CreateProductCommand> createProductHandler,
            IHandler<DeleteProductCommand> deleteProductHandler,
            IHandler<UpdateProductCommand> updateProductHandler
            )
        {
            _productRepository = productRepository;
            _createProductHandler = createProductHandler;
            _deleteProductHandler = deleteProductHandler;
            _updateProductHandler = updateProductHandler;
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
            var products = await _productRepository.ReadAllAsync();
            List<ProductViewModel> productsViewModel = new();

            foreach(var product in products)
            {
                productsViewModel.Add(GetViewModelFromEntity(product));
            }

            return productsViewModel;
        }

        public async Task<ProductViewModel> GetProductAsync(int id)
        {
            var product = await _productRepository.ReadAsync(id);
            
            if(product is null)
                throw new ApplicationException(ProductServiceMessages.ERROR_PRODUCT_ID_NOT_FOUND);

            ProductViewModel ProductViewModel = GetViewModelFromEntity(product);

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

        private ProductViewModel GetViewModelFromEntity(Product product)
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
