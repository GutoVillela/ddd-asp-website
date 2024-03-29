﻿using KadoshWebsite.Infrastructure;
using KadoshWebsite.Infrastructure.Authentication;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public class CustomerPostingsController : Controller
    {

        private readonly ICustomerPostingApplicationService _customerPostingsService;

        public CustomerPostingsController(ICustomerPostingApplicationService customerPostingsService)
        {
            _customerPostingsService = customerPostingsService;
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<PartialViewResult> GetCustomerPostingsByCustomerPaginatedAsync(int? page, int? filterByCustumerId)
        {
            PaginatedListViewModel<CustomerPostingViewModel> customerPostings = new();

            if(filterByCustumerId.HasValue)
                customerPostings = await _customerPostingsService.GetAllPostingsFromCustomerPaginatedAsync(filterByCustumerId.Value, page ?? 1, PaginationManager.PAGE_SIZE);

            return PartialView("_CustomerPostingsListTable", customerPostings);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<PartialViewResult> GetCustomerPostingsBySalePaginatedAsync(int? page, int? filterBySaleId)
        {
            PaginatedCustomerPostingsViewModel customerPostings = new();

            if (filterBySaleId.HasValue)
                customerPostings = await _customerPostingsService.GetAllPostingsFromSalePaginatedAsync(filterBySaleId.Value, page ?? 1, PaginationManager.PAGE_SIZE);

            return PartialView("_CustomerPostingsListTable", customerPostings);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<PartialViewResult> GetCustomerPostingsByStoreAndDatePaginatedAsync(int? page, DateTime? date, int? storeId, bool getTotal)
        {
            PaginatedCustomerPostingsViewModel customerPostings = new();

            if (date.HasValue && storeId.HasValue)
                customerPostings = await _customerPostingsService.GetAllPostingsFromStoreAndDatePaginatedAsync(
                    DateOnly.FromDateTime(date.Value),
                    FormatProviderManager.TimeZone,
                    storeId.Value, 
                    getTotal,
                    page ?? 1, 
                    PaginationManager.PAGE_SIZE);

            return PartialView("_CustomerPostingsListTable", customerPostings);
        }

        [HttpGet]
        [Authorize(Roles = Roles.CUSTOMER_ROLE)]
        public async Task<IActionResult> GetCustomerPostingsInfoByCustomerPaginatedAsync(int custumerId, int page, int pageSize)
        {
            try
            {
                PaginatedListViewModel<CustomerPostingViewModel> customerPostings = 
                    await _customerPostingsService.GetAllPostingsFromCustomerPaginatedAsync(custumerId, page, pageSize);

                return Ok(customerPostings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
