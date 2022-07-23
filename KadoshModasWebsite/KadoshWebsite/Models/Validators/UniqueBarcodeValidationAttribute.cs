using KadoshDomain.Queries.ProductQueries.GetProductByBarCode;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace KadoshWebsite.Models.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UniqueBarcodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IServiceProvider serviceProvider = ServiceProviderManager.ServiceProvider;
            var getProductByBarCodeQueryHandler = serviceProvider.GetService<IQueryHandler<GetProductByBarCodeQuery, GetProductByBarCodeQueryResult>>();

            if(value is null)
                return ValidationResult.Success;

            GetProductByBarCodeQuery query = new();
            query.BarCode = value.ToString();

            var result = getProductByBarCodeQueryHandler.HandleAsync(query).Result;

            if (result.Success)
                return new ValidationResult(ErrorMessage);

            if(result.Errors!.Any(x => x.Code == ErrorCodes.ERROR_PRODUCT_NOT_FOUND))
                return ValidationResult.Success;

            throw new ApplicationException(result.Errors!.GetAsSingleMessage());
        }
    }
}
