using Currencies.Common;
using Currencies.Data.Context;
using Currencies.Models.Currency;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public interface ICurrencyService
    {
        Task<ConversionResult> Convert(ConversionInstruction conversionInfo);
        Task<List<Symbol>> GetAvailableCurrencies();
        Task<List<Rate>> GetCurrentRates(string baseCurrnecy);
        Task<List<Rate>> GetHistoricalRates(int days);
    }

    public class CurrencyService : ICurrencyService
    // : ICurrencyService
    {
        private readonly ExchangeService _exchangeService;
        private readonly IMemoryCache _memoryCache;
        private readonly DataContext _context;

        public CurrencyService(ExchangeService exchangeService, IMemoryCache memoryCache, DataContext context)
        {
            this._exchangeService = exchangeService;
            this._memoryCache = memoryCache;
            this._context = context;
        }

        public async Task<List<Symbol>> GetAvailableCurrencies()
        {
            if (!_memoryCache.TryGetValue(Constants.CacheKeys.CURRENCIES, out dynamic cacheValue))
            {
                var response = await _exchangeService.GetSupportedCurrencies();

                if (!response.IsSuccess)
                    return null;

                dynamic jObject = response.Result;
                var success = jObject["success"];
                if (!success)
                    return null;

                Dictionary<string, object> symbols = jObject["symbols"];
                var convertedSymbols = symbols.Select(kvp =>
                {
                    return new Symbol
                    {
                        Key = kvp.Key,
                        Name = kvp.Value.ToString()
                    };
                }).ToList();
                CacheExtensions.Set(_memoryCache, Constants.CacheKeys.CURRENCIES, convertedSymbols, TimeSpan.FromMinutes(Constants.CACHE_MINUTES));

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
            var date = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
            if (!_memoryCache.TryGetValue($"{Constants.CacheKeys.HISTORIES}@{date}", out dynamic cacheValue))
            {
                var response = await _exchangeService.GetHistoricalRates(date);

                if (!response.IsSuccess)
                    return null;

                dynamic jObject = response.Result;
                var success = jObject["success"];
                if (!success)
                    return null;


                Dictionary<string, object> rates = jObject["rates"];
                var convertedRates = rates.Select(kvp =>
                {
                    return new Rate
                    {
                        Symbol = kvp.Key,
                        Value = System.Convert.ToDouble(kvp.Value)
                    };
                }).ToList();

                CacheExtensions.Set(_memoryCache, $"{Constants.CacheKeys.HISTORIES}@{date}", convertedRates, TimeSpan.FromMinutes(Constants.CACHE_MINUTES));

                return convertedRates;
            }
            return cacheValue;
        }

        public async Task<List<Rate>> GetCurrentRates(string baseCurrnecy)
        {
            if (!_memoryCache.TryGetValue(Constants.CacheKeys.RATES, out dynamic cacheValue))
            {
                var response = await _exchangeService.GetCurrentRates(baseCurrnecy);

                if (!response.IsSuccess)
                    return null;

                dynamic jObject = response.Result;
                var success = jObject["success"];
                if (!success)
                    return null;


                Dictionary<string, object> rates = jObject["rates"];

                var convertedRates = rates.Select(kvp =>
                {
                    return new Rate
                    {
                        Symbol = kvp.Key,
                        Value = System.Convert.ToDouble(kvp.Value)
                    };
                }).ToList();

                CacheExtensions.Set(_memoryCache, Constants.CacheKeys.RATES, convertedRates, TimeSpan.FromMinutes(Constants.CACHE_MINUTES));
                SaveRatesToDatabase(baseCurrnecy, convertedRates);
                return convertedRates;
            }
            return cacheValue;
        }

        private void SaveRatesToDatabase(string currency, List<Rate> rates) {
            var mappedRates = Mapping.Mapper.Map<List<Data.Entities.Rate>>(rates);
            _context.CurrencyRates.Add(
                    new Data.Entities.CurrencyRate { 
                        BaseCurrency = currency,
                        Time = DateTime.UtcNow,
                        Rates = mappedRates
                    }
                );
            _context.SaveChanges();
        }
    }
}
