using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public interface ICurrencyService {
        Task<Dictionary<string, object>> GetAvailableCurrencies();
        Task GetCurrentRates(string baseCurrnecy);
    }
    public class CurrencyService// : ICurrencyService
    {
        private readonly ExchangeService exchangeService;

        public CurrencyService(ExchangeService exchangeService)
        {
            this.exchangeService = exchangeService;

        }

        public async Task<Dictionary<string, object>> GetAvailableCurrencies()
        {
            var response = await exchangeService.GetSupportedCurrencies();
            //dynamic jObject = JObject.Parse("{\"success\": true,\"symbols\": {  \"AED\": \"United Arab Emirates Dirham\",  \"AFN\": \"Afghan Afghani\",  \"ALL\": \"Albanian Lek\",  \"AMD\": \"Armenian Dram\"  }}");

            if (!response.IsSuccess)
                return null;

            dynamic jObject = response.Result;
            var success = jObject["success"];
            if (!success)
                return null;


            var symbols = jObject["symbols"];//.ToObject<Dictionary<string, object>>();
            

            return symbols;
        }

        public async Task<Dictionary<string, object>> GetHistoricalRates(int days)
        {
            var date = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
            var response = await exchangeService.GetHistoricalRates(date);
            //dynamic jObject = JObject.Parse("{\"success\": true,\"symbols\": {  \"AED\": \"United Arab Emirates Dirham\",  \"AFN\": \"Afghan Afghani\",  \"ALL\": \"Albanian Lek\",  \"AMD\": \"Armenian Dram\"  }}");

            if (!response.IsSuccess)
                return null;

            dynamic jObject = response.Result;
            var success = jObject["success"];
            if (!success)
                return null;


            var symbols = jObject["rates"];//.ToObject<Dictionary<string, object>>();


            return symbols;
        }

        public async Task<Dictionary<string, object>> GetCurrentRates(string baseCurrnecy)
        { 
            var response = await exchangeService.GetCurrentRates(baseCurrnecy);
            //dynamic jObject = JObject.Parse("{\"success\": true,\"symbols\": {  \"AED\": \"United Arab Emirates Dirham\",  \"AFN\": \"Afghan Afghani\",  \"ALL\": \"Albanian Lek\",  \"AMD\": \"Armenian Dram\"  }}");

            if (!response.IsSuccess)
                return null;

            dynamic jObject = response.Result;
            var success = jObject["success"];
            if (!success)
                return null;


            var symbols = jObject["rates"];//.ToObject<Dictionary<string, object>>();


            return symbols;
        }
    }
}
