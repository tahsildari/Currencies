namespace Currencies.Models
{
    public class ExchangeSettings
    {
        public string AccessKey { get; set; }
        public string ApiBaseAddress { get; set; }
        public string CurrenciesEndpoint { get; set; }
        public string CurrencyEndpoint { get; set; }
        public string HistoryEndpoint { get; set; }
    }
}
