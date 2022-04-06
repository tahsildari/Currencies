using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Currencies.Data.Entities
{
    public class CurrencyRate
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string BaseCurrency { get; set; }

        public List<Rate> Rates { get; set; }
    }

    [Owned]
    public class Rate {
        public string Symbol { get; set; }
        public double Value { get; set; }
    }
}
