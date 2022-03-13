﻿using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.ServicesMessages;
using KadoshShared.Handlers;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.BrandQueries.GetBrandById
{
    public class GetBrandByIdQueryHandler : QueryHandlerBase<GetBrandByIdQuery, GetBrandByIdQueryResult>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandByIdQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public override async Task<GetBrandByIdQueryResult> HandleAsync(GetBrandByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_BRAND_BY_ID_QUERY);
                return new GetBrandByIdQueryResult(errors as IReadOnlyCollection<Error>);
            }

            var brand = await _brandRepository.ReadAsync(command.BrandId!.Value);

            if (brand is null)
            {
                AddNotification(nameof(brand), BrandServiceMessages.ERROR_BRAND_ID_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_BRAND_NOT_FOUND);
                return new GetBrandByIdQueryResult(errors as IReadOnlyCollection<Error>);
            }

            GetBrandByIdQueryResult brandViewModel = new()
            {
                Brand = brand
            };

            return brandViewModel;
        }


    }
}
