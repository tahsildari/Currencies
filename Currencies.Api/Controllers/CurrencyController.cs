using Currencies.Models.Currency;
using Currencies.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Currencies.Api.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAvailableCurrencies()
        {
            var currencies = await _currencyService.GetAvailableCurrencies();
            return Ok(currencies);
        }

        [HttpGet]
        [Route("{baseCurrency}")]
        public async Task<IActionResult> GetCurrentRates([FromRoute] string baseCurrency)
        {
            var currencies = await _currencyService.GetCurrentRates(baseCurrency);
            return Ok(currencies);
        }

        [HttpGet]
        [Route("/historic/{days}")]
        public async Task<IActionResult> GetHistoricalRates([FromRoute] int days)
        {
            var currencies = await _currencyService.GetHistoricalRates(days);
            return Ok(currencies);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Convert([FromBody] ConversionInstruction conversionInfo)
        {
            var currencies = await _currencyService.Convert(conversionInfo);
            return Ok(currencies);
        }

        
    }
}
