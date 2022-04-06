using System;
using System.Collections.Generic;
using System.Text;

namespace Currencies.Models.Currency
{
    public class ConversionResult : ConversionBase
    {
        public double BaseValue { get; set; }
        public double Result { get; set; }
    }
}
