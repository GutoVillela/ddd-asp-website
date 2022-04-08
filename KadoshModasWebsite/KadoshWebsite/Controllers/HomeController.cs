using KadoshWebsite.Infrastructure;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KadoshWebsite.Controllers
{
    [Authorize(Policy = nameof(LoggedInAuthorization))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportService _reportService;

        public HomeController(ILogger<HomeController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetWeekSellsAsync()
        {
            try
            {
                int salesOfTheWeekCount = await _reportService.GetWeekSellsCountAsync(FormatProviderManager.TimeZone);
                return Ok(salesOfTheWeekCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalToReceiveFromSalesAsync()
        {
            try
            {
                decimal totalToReceive = await _reportService.GetTotalToReceiveFromSalesAsync();
                return Ok(totalToReceive.ToString("C", FormatProviderManager.CultureInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDelinquentCustomersCountAsync()
        {
            try
            {
                int delinquentCustomersCount = await _reportService.GetDelinquentCustomersCountAsync(FormatProviderManager.INTERVAL_BEFORE_DELINQUENT_IN_DAYS);
                return Ok(delinquentCustomersCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSellsFromMonthToChartAsync()
        {
            try
            {
                var monthSalesReport = await _reportService.GetAllSalesFromLast30DaysAsync(FormatProviderManager.TimeZone);
                return Ok(monthSalesReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}