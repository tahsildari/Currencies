using Currencies.Common;
using Currencies.Common.Utilities;
using Currencies.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public class ExchangeService
    {
        private readonly IRestClient _client;
        private ExchangeSettings _exchangeSettings;
        private string _currenciesUrl;
        private string _currencyUrl;
        private string _historyUrl;
        public ExchangeService(IOptions<ExchangeSettings> exchangeSettings, IRestClient client)
        {
            _exchangeSettings = exchangeSettings.Value;
            _client = client;
            SetUpEndpoints();
        }


        public async Task<ApiResult<object>> GetSupportedCurrencies() {
            var uri = _currenciesUrl.ToUri();
            IRestRequest request = new RestRequest(uri);
            request.Method = Method.GET;
            return await _client.CallApiAsync<object>(request);
        }

        private void SetUpEndpoints()
        {
            var ex = _exchangeSettings;
            _currenciesUrl = (ex.ApiBaseAddress + ex.CurrenciesEndpoint)
                .Replace(Constants.API_KEY, ex.AccessKey);
            _currencyUrl = (ex.ApiBaseAddress + ex.CurrencyEndpoint)
                .Replace(Constants.API_KEY, ex.AccessKey);
            _historyUrl = (ex.ApiBaseAddress + ex.HistoryEndpoint)
                .Replace(Constants.API_KEY, ex.AccessKey);
        }

        internal async Task<ApiResult<object>> GetCurrentRates(string baseCurrnecy)
        {
            var uri = _currencyUrl.Replace(Constants.BASE, baseCurrnecy).ToUri();
            IRestRequest request = new RestRequest(uri);
            request.Method = Method.GET;
            return await _client.CallApiAsync<object>(request);
        }

        internal async Task<ApiResult<object>> GetHistoricalRates(string date)
        {
            var uri = _historyUrl
                .Replace(Constants.DATE, date)
                .Replace(Constants.BASE, Constants.EUR)
                .ToUri();
            IRestRequest request = new RestRequest(uri);
            request.Method = Method.GET;
            return await _client.CallApiAsync<object>(request);
        }
    }
}
