using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Repositories;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductApplicationService(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<ICommandResult> CreateProductAsync(ProductViewModel Product)
        {
            CreateProductCommand command = new();
            command.Name = Product.Name;
            command.BarCode = Product.BarCode;
            command.Price = Product.Price;
            command.CategoryId = Product.CategoryId;
            command.BrandId = Product.BrandId;

            ProductHandler productHandler = new(_unitOfWork, _productRepository);
            return await productHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteProductAsync(int id)
        {
            DeleteProductCommand command = new();
            command.Id = id;

            ProductHandler productHandler = new(_unitOfWork, _productRepository);
            return await productHandler.HandleAsync(command);
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

            ProductHandler productHandler = new(_unitOfWork, _productRepository);
            return await productHandler.HandleAsync(command);
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
