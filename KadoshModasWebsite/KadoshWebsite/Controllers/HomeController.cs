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
                int salesOfTheWeekCount = await _reportService.GetWeekSellsAsync(FormatProviderManager.TimeZone);
                return Ok(salesOfTheWeekCount);
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