using System;
using System.Collections.Generic;
using System.Text;

namespace Currencies.Services
{
    public static class Constants
    {
        public const string APIKEY = "{API_KEY}";
        public const string DATE = "{date}";
        public const string BASE = "{base}";
        public const string EUR = "EUR";

        public static class CacheKeys { 
            public const string CURRENCIES = "currencies";
            public const string HISTORIES = "histories";
            public const string RATES = "rates";
        }

        public const int CACHEMINUTES = 15;
    }
}
