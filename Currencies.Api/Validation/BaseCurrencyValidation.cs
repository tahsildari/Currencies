using Currencies.Common;
using FluentValidation;
using System;

namespace Currencies.Api.Validation
{
    public class BaseCurrencyValidation : AbstractValidator<string>
    {
        public BaseCurrencyValidation()
        {
            RuleFor(t => t)
                .NotEmpty().WithMessage(Constants.ValidationMessages.CURRENCY_BASE_REQUIRED)
                .Equal("EUR", StringComparer.CurrentCultureIgnoreCase).WithMessage(Constants.ValidationMessages.NON_EUR_BASE);
        }
    }
}
