using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;
using KadoshDomain.LegacyEntities;
using KadoshDomain.ExtensionMethods;
using KadoshDomain.Enums;

namespace KadoshDomain.Commands.SettingsCommands.ImportDataFromLegacy
{
    public class ImportDataFromLegacyHandler : CommandHandlerBase<ImportDataFromLegacyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        // Repositories
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _postingRepository;

        // Legacy repositories
        private readonly ILegacyRepository<Brand, BrandLegacy> _brandLegacyRepository;
        private readonly ILegacyRepository<Category, CategoryLegacy> _categoryLegacyRepository;
        private readonly ILegacyRepository<Product, ProductLegacy> _productLegacyRepository;
        private readonly ILegacyRepository<Customer, CustomerLegacy> _customerLegacyRepository;
        private readonly ILegacyRepository<Sale, SaleLegacy> _saleLegacyRepository;
        private readonly ILegacyRepository<CustomerPosting, CustomerPostingLegacy> _postingLegacyRepository;

        public ImportDataFromLegacyHandler(
            IUnitOfWork unitOfWork,
            IStoreRepository storeRepository,
            IUserRepository userRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            ISaleRepository saleRepository,
            ICustomerPostingRepository postingRepository,
            ILegacyRepository<Brand, BrandLegacy> brandLegacyRepository,
            ILegacyRepository<Category, CategoryLegacy> categoryLegacyRepository,
            ILegacyRepository<Product, ProductLegacy> productLegacyRepository,
            ILegacyRepository<Customer, CustomerLegacy> customerLegacyRepository,
            ILegacyRepository<Sale, SaleLegacy> saleLegacyRepository,
            ILegacyRepository<CustomerPosting, CustomerPostingLegacy> postingLegacyRepository
            )
        {
            _unitOfWork = unitOfWork;

            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _saleRepository = saleRepository;
            _postingRepository = postingRepository;

            _brandLegacyRepository = brandLegacyRepository;
            _categoryLegacyRepository = categoryLegacyRepository;
            _productLegacyRepository = productLegacyRepository;
            _customerLegacyRepository = customerLegacyRepository;
            _saleLegacyRepository = saleLegacyRepository;
            _postingLegacyRepository = postingLegacyRepository;
        }

        public override async Task<ICommandResult> HandleAsync(ImportDataFromLegacyCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);

                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_IMPORT_FROM_LEGACY_COMMAND);
                    return new CommandResult(false, ImportFromLegacyCommandMessages.INVALID_IMPORT_FROM_LEGACY_COMMAND, errors);
                }

                // Retrieve default options 
                #region Validations
                // TODO Isolate all validations in another method
                var store = await _storeRepository.ReadAsync(command.DefaultStoreId!.Value);

                if (store == null)
                {
                    AddNotification(nameof(store), ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_STORE);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_IMPORT_FROM_LEGACY_STORE);
                    return new CommandResult(false, ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_STORE, errors);
                }

                var defaultSeller = await _userRepository.ReadAsync(command.DefaultSellerId!.Value);

                if (defaultSeller == null)
                {
                    AddNotification(nameof(defaultSeller), ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_SELLER);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_IMPORT_FROM_LEGACY_SELLER);
                    return new CommandResult(false, ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_SELLER, errors);
                }

                var defaultBrand = await _brandRepository.ReadAsync(command.DefaultBrandId!.Value);

                if (defaultBrand == null)
                {
                    AddNotification(nameof(defaultBrand), ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_BRAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_IMPORT_FROM_LEGACY_BRAND);
                    return new CommandResult(false, ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_BRAND, errors);
                }

                var defaultCategory = await _categoryRepository.ReadAsync(command.DefaultCategoryId!.Value);

                if (defaultCategory == null)
                {
                    AddNotification(nameof(defaultCategory), ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_CATEGORY);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_IMPORT_FROM_LEGACY_CATEGORY);
                    return new CommandResult(false, ImportFromLegacyCommandMessages.COULD_NOT_FIND_IMPORT_FROM_LEGACY_CATEGORY, errors);
                }
                #endregion Validations

                string legacyConnectionString = $"Server={command.Server}; Initial Catalog={command.LegacyDatabaseName}; Integrated Security=true";

                // Import all Brands
                var importedBrands = await ImportBrandsFromLegacyAsync(legacyConnectionString);

                // Import all Categories
                var importedCategories = await ImportCategoriesFromLegacyAsync(legacyConnectionString);

                // Import all Products
                var importedProducts = await ImportProductsFromLegacyAsync(legacyConnectionString, importedBrands, defaultBrand, importedCategories, defaultCategory);

                // Import all Customers
                var importedCustomers = await ImportCustomersFromLegacyAsync(legacyConnectionString);

                // Import all Sales (with Installments AND Sale Items)
                var importedSales = await ImportSalesFromLegacyAsync(legacyConnectionString, importedCustomers, importedProducts, store, defaultSeller);

                // TODO Import all Customer Postings
                var importedCustomerPostings = await ImportCustomerPostingsFromLegacyAsync(legacyConnectionString, importedSales);

                // TODO Create missing Customer Postings
                await CreateMissingPostingsAsync(importedSales, importedCustomerPostings);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, ImportFromLegacyCommandMessages.SUCCESS_ON_IMPORT_FROM_LEGACY_COMMAND);
            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, string.Format(ImportFromLegacyCommandMessages.UNEXPECTED_EXCEPTION_ON_IMPORT_FROM_LEGACY_COMMAND, ex.Message), errors);
            }
        }

        private async Task<IList<ImportFromLegacyMap<Brand, BrandLegacy>>> ImportBrandsFromLegacyAsync(string legacyConnectionString)
        {
            List<ImportFromLegacyMap<Brand, BrandLegacy>> importMapList = new();

            // Read all from legacy repository
            var brandsFromLegacy = await _brandLegacyRepository.ReadAllAsync(legacyConnectionString);
            foreach(var brandLegacy in brandsFromLegacy)
            {
                importMapList.Add(new(brandLegacy, brandLegacy));
            }

            // Save all in destiny repository
            foreach(var brand in importMapList)
            {
                await _brandRepository.CreateAsync(brand.ImportedEntity);
            }

            return importMapList;
        }

        private async Task<IList<ImportFromLegacyMap<Category, CategoryLegacy>>> ImportCategoriesFromLegacyAsync(string legacyConnectionString)
        {
            List<ImportFromLegacyMap<Category, CategoryLegacy>> importMapList = new();

            // Read all from legacy repository
            var categoriesFromLegacy = await _categoryLegacyRepository.ReadAllAsync(legacyConnectionString);
            foreach (var categoryLegacy in categoriesFromLegacy)
            {
                importMapList.Add(new(categoryLegacy, categoryLegacy));
            }

            // Save all in destiny repository
            foreach (var category in importMapList)
            {
                await _categoryRepository.CreateAsync(category.ImportedEntity);
            }

            return importMapList;
        }

        private async Task<IList<ImportFromLegacyMap<Product, ProductLegacy>>> ImportProductsFromLegacyAsync(
            string legacyConnectionString,
            IList<ImportFromLegacyMap<Brand, BrandLegacy>> importedBrands,
            Brand defaultBrand,
            IList<ImportFromLegacyMap<Category, CategoryLegacy>> importedCategories,
            Category defaultCategory
            )
        {
            List<ImportFromLegacyMap<Product, ProductLegacy>> importMapList = new();

            // Read all from legacy repository
            var productsFromLegacy = await _productLegacyRepository.ReadAllAsync(legacyConnectionString);
            foreach (var productLegacy in productsFromLegacy)
            {
                importMapList.Add(new(productLegacy, productLegacy));
            }

            // Save all in destiny repository
            foreach (var product in importMapList)
            {
                // Check product brand
                Brand brand = defaultBrand;
                if (!string.IsNullOrEmpty(product.LegacyEntity.Brand) && importedBrands.Any(x => x.LegacyEntity.Name == product.LegacyEntity.Brand))
                    brand = importedBrands.First(x => x.LegacyEntity.Name == product.LegacyEntity.Brand).ImportedEntity;

                // Check product category
                Category category = defaultCategory;
                if (!string.IsNullOrEmpty(product.LegacyEntity.Category) && importedCategories.Any(x => x.LegacyEntity.Name == product.LegacyEntity.Category))
                    category = importedCategories.First(x => x.LegacyEntity.Name == product.LegacyEntity.Category).ImportedEntity;

                product.ImportedEntity.SetBrand(brand);
                product.ImportedEntity.SetCategory(category);
                await _productRepository.CreateAsync(product.ImportedEntity);
            }

            return importMapList;
        }

        private async Task<IList<ImportFromLegacyMap<Customer, CustomerLegacy>>> ImportCustomersFromLegacyAsync(string legacyConnectionString)
        {
            List<ImportFromLegacyMap<Customer, CustomerLegacy>> importMapList = new();

            // Read all from legacy repository
            var customersFromLegacy = await _customerLegacyRepository.ReadAllAsync(legacyConnectionString);
            foreach (var customerLegacy in customersFromLegacy)
            {
                importMapList.Add(new(customerLegacy, customerLegacy));
            }

            // Save all in destiny repository
            foreach (var customer in importMapList)
            {
                await _customerRepository.CreateAsync(customer.ImportedEntity);
            }

            return importMapList;
        }

        private async Task<IList<ImportFromLegacyMap<Sale, SaleLegacy>>> ImportSalesFromLegacyAsync(
            string legacyConnectionString,
            IList<ImportFromLegacyMap<Customer, CustomerLegacy>> importedCustomers,
            IList<ImportFromLegacyMap<Product, ProductLegacy>> importedProducts,
            Store defaultStore,
            User defaultSeller
            )
        {
            List<ImportFromLegacyMap<Sale, SaleLegacy>> importMapList = new();

            // Read all from legacy repository
            var salesFromLegacy = await _saleLegacyRepository.ReadAllAsync(legacyConnectionString);
            foreach (var saleLegacy in salesFromLegacy)
            {
                importMapList.Add(new(saleLegacy, saleLegacy));
            }

            // Save all in destiny repository
            foreach (var sale in importMapList)
            {
                if (!importedCustomers.Any(x => x.LegacyEntity.Id == sale.LegacyEntity.CustomerId))
                    throw new ApplicationException(ImportFromLegacyCommandMessages.CUSTOMER_ID_NOT_FIND_FROM_IMPORTED_CUSTOMERS);

                Customer customer = importedCustomers.First(x => x.LegacyEntity.Id == sale.LegacyEntity.CustomerId).ImportedEntity;
                sale.ImportedEntity.SetCustomer(customer);
                sale.ImportedEntity.SetSeller(defaultSeller);
                sale.ImportedEntity.SetStore(defaultStore);
                sale.ImportedEntity.SetSaleItems(FixSaleItemsProduct(sale.LegacyEntity.SaleItems, importedProducts));
                await _saleRepository.CreateAsync(sale.ImportedEntity);
            }

            return importMapList;
        }

        private IEnumerable<SaleItem> FixSaleItemsProduct(IEnumerable<SaleItemLegacy> saleItemsLegacy, IList<ImportFromLegacyMap<Product, ProductLegacy>> importedProducts)
        {
            List<SaleItem> saleItems = new();
            foreach(var item in saleItemsLegacy)
            {
                if(!importedProducts.Any(x => x.LegacyEntity.Id == item.ProductId))
                    throw new ApplicationException(ImportFromLegacyCommandMessages.PRODUCT_ID_NOT_FIND_FROM_IMPORTED_PRODUCTS);

                var product = importedProducts.First(x => x.LegacyEntity.Id == item.ProductId).ImportedEntity;
                SaleItem saleItem = item;
                saleItems.Add(new SaleItem(product, saleItem.Amount, saleItem.Price, saleItem.DiscountInPercentage, saleItem.Situation));
            }
            return saleItems;
        }

        private async Task<IList<ImportFromLegacyMap<CustomerPosting, CustomerPostingLegacy>>> ImportCustomerPostingsFromLegacyAsync(string legacyConnectionString, IList<ImportFromLegacyMap<Sale, SaleLegacy>> importedSales)
        {
            List<ImportFromLegacyMap<CustomerPosting, CustomerPostingLegacy>> importMapList = new();

            // Read all from legacy repository
            var postingsFromLegacy = await _postingLegacyRepository.ReadAllAsync(legacyConnectionString);
            foreach (var posting in postingsFromLegacy)
            {
                importMapList.Add(new(posting, posting));
            }

            // Save all in destiny repository
            foreach (var posting in importMapList)
            {
                // Get correct sale
                Sale sale = GetSaleFromCustomerPosting(posting.LegacyEntity.SaleId, posting.LegacyEntity.CustomerId, importedSales);
                await CreatePostingAsync(sale, posting.ImportedEntity.Type, posting.ImportedEntity.Value, posting.ImportedEntity.PostingDate);
            }

            return importMapList;
        }

        private Sale GetSaleFromCustomerPosting(int legacySaleId, int legacyCustomerId, IList<ImportFromLegacyMap<Sale, SaleLegacy>> importedSales)
        {
            Sale sale;

            // If there's no sale associated get the first sale from customer
            if (legacySaleId == 0)
            {
                if(!importedSales.Any(x => x.LegacyEntity.CustomerId == legacyCustomerId))
                    throw new ApplicationException(ImportFromLegacyCommandMessages.CUSTOMER_ID_NOT_FIND_FROM_IMPORTED_CUSTOMER_POSTING);

                sale = importedSales.First(x => x.LegacyEntity.CustomerId == legacyCustomerId).ImportedEntity;
            }
            else
            {
                if(!importedSales.Any(x => x.LegacyEntity.Id == legacySaleId))
                    throw new ApplicationException(ImportFromLegacyCommandMessages.SALE_ID_NOT_FIND_FROM_IMPORTED_SALES);

                sale = importedSales.First(x => x.LegacyEntity.Id == legacySaleId).ImportedEntity;
            }

            return sale;
        }

        private async Task CreatePostingAsync(Sale sale, ECustomerPostingType type, decimal value, DateTime postingDate)
        {
            CustomerPosting importedPosting = new(
                    type: type,
                    value: value,
                    sale: sale,
                    postingDate: postingDate
                    );
            await _postingRepository.CreateAsync(importedPosting);
        }

        private async Task CreateMissingPostingsAsync(IList<ImportFromLegacyMap<Sale, SaleLegacy>> importedSales, IList<ImportFromLegacyMap<CustomerPosting, CustomerPostingLegacy>> importedPostings)
        {
            foreach(var sale in importedSales)
            {
                var salePostings = importedPostings.Where(x => x.LegacyEntity.SaleId == sale.LegacyEntity.Id && x.ImportedEntity.Type.IsCreditType());
                var totalPostingValues = salePostings.Sum(x => x.LegacyEntity.Value);

                // Create postings to complement paid value
                if(totalPostingValues < sale.LegacyEntity.Paid)
                {
                    decimal difference = sale.LegacyEntity.Paid - totalPostingValues;
                    await CreatePostingAsync(sale.ImportedEntity, ECustomerPostingType.Payment, difference, DateTime.UtcNow);
                }
            }
        }
    }
}