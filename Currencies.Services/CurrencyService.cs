using Currencies.Models.Currency;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, object>> GetAvailableCurrencies();
        Task GetCurrentRates(string baseCurrnecy);
    }
    public class CurrencyService// : ICurrencyService
    {
        private readonly ExchangeService exchangeService;
        private readonly IMemoryCache _memoryCache;

        public CurrencyService(ExchangeService exchangeService, IMemoryCache memoryCache)
        {
            this.exchangeService = exchangeService;
            this._memoryCache = memoryCache;
        }

        public async Task<List<Symbol>> GetAvailableCurrencies()
        {
            if (!_memoryCache.TryGetValue(Constants.CacheKeys.CURRENCIES, out dynamic cacheValue))
            {
                var response = await exchangeService.GetSupportedCurrencies();

                if (!response.IsSuccess)
                    return null;

                dynamic jObject = response.Result;
                var success = jObject["success"];
                if (!success)
                    return null;

                Dictionary<string, object> symbols = jObject["symbols"];
                var convertedSymbols = symbols.Select(kvp => {
                    return new Symbol
                    {
                        Key = kvp.Key,
                        Name = kvp.Value.ToString()
                    };
                }).ToList();
                CacheExtensions.Set(_memoryCache, Constants.CacheKeys.CURRENCIES, convertedSymbols, TimeSpan.FromMinutes(Constants.CACHEMINUTES));

                return convertedSymbols;
            }
            return cacheValue;
        }

        public async Task<ConversionResult> Convert(ConversionInstruction conversionInfo)
        {
            var rates = await GetCurrentRates(conversionInfo.FromCurrency);
            var targetRate = rates.Where(r => r.Symbol == conversionInfo.ToCurrency).FirstOrDefault();

            var calculatedValue = conversionInfo.Value * targetRate.Value;
            return new ConversionResult
            {
                FromCurrency = conversionInfo.FromCurrency,
                ToCurrency = conversionInfo.ToCurrency,
                BaseValue = conversionInfo.Value,
                Result = calculatedValue
            };
        }

        public async Task<List<Rate>> GetHistoricalRates(int days)
        {
            if (!_memoryCache.TryGetValue(Constants.CacheKeys.HISTORIES, out dynamic cacheValue))
            {
                var date = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
                var response = await exchangeService.GetHistoricalRates(date);

                if (!response.IsSuccess)
                    return null;

                dynamic jObject = response.Result;
                var success = jObject["success"];
                if (!success)
                    return null;


                Dictionary<string, object> rates = jObject["rates"];
                var convertedRates = rates.Select(kvp => {
                    return new Rate
                    {
                        Symbol = kvp.Key,
                        Value = System.Convert.ToDouble(kvp.Value)
                    };
                }).ToList();

                CacheExtensions.Set(_memoryCache, Constants.CacheKeys.HISTORIES, convertedRates, TimeSpan.FromMinutes(Constants.CACHEMINUTES));

                return convertedRates;
            }
            return cacheValue;
        }

        public async Task<List<Rate>> GetCurrentRates(string baseCurrnecy)
        {
            if (!_memoryCache.TryGetValue(Constants.CacheKeys.RATES, out dynamic cacheValue))
            {
                var response = await exchangeService.GetCurrentRates(baseCurrnecy);

                if (!response.IsSuccess)
                    return null;

                dynamic jObject = response.Result;
                var success = jObject["success"];
                if (!success)
                    return null;


                Dictionary<string, object> rates = jObject["rates"];

                var convertedRates = rates.Select(kvp=> {
                    return new Rate
                    {
                        Symbol = kvp.Key,
                        Value = System.Convert.ToDouble(kvp.Value)
                    };
                }).ToList();

                CacheExtensions.Set(_memoryCache, Constants.CacheKeys.RATES, convertedRates, TimeSpan.FromMinutes(Constants.CACHEMINUTES));

                return convertedRates;
            }
            return cacheValue;
        }
    }
}
