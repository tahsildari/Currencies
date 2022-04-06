using System;
using System.Collections.Generic;
using System.Text;

namespace Currencies.Models.Currency
{
    public class ConversionBase
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
    }
}
