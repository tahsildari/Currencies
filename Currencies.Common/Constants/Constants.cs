namespace Currencies.Common
{
    public static class Constants
    {
        public const string API_KEY = "{API_KEY}";
        public const string DATE = "{date}";
        public const string BASE = "{base}";
        public const string EUR = "EUR";

        public static class CacheKeys
        {
            public const string CURRENCIES = "currencies";
            public const string HISTORIES = "histories";
            public const string RATES = "rates";
        }

        public const int CACHE_MINUTES = 15;

        public static class ValidationMessages
        {
            public const string CURRENCY_BASE_REQUIRED = "Base currency is required!";
            public const string NON_EUR_BASE = "Since you are on free api subscription, only use EUR as base currency!";
            public const string HISTORY_CANNOT_BE_ZERO = "Zero is not valid for days!";
            public const string HISTORY_MUST_BE_POSITIVE = "Enter a positive number of days!";
            public const string FROM_CURRENCY_REQUIRED = "From currency is required!";
            public const string FROM_CURRENCY_LEN = "From currency's length should be 3!";
            public const string TO_CURRENCY_REQUIRED = "To currency is required!";
            public const string TO_CURRENCY_LEN = "From currency's length should be 3!";
            public const string INVALID_VALUE = "Conversion Value must be a positive number!";
            public const string CONVERSION_CURRENCIES_MUST_DIFFER = "Conversion currencies must be different!";

        }
    }
}
