using Currencies.Common;
using Currencies.Models.Currency;
using FluentValidation;
using System;

namespace Currencies.Api.Validation
{
    public class ConversionValidation : AbstractValidator<ConversionInstruction>
    {
        public ConversionValidation()
        {
            RuleFor(t => t.FromCurrency)
                .NotEmpty().WithMessage(Constants.ValidationMessages.FROM_CURRENCY_REQUIRED)
                .Length(3).WithMessage(Constants.ValidationMessages.FROM_CURRENCY_LEN);

            RuleFor(t => t.ToCurrency)
                .NotEmpty().WithMessage(Constants.ValidationMessages.TO_CURRENCY_REQUIRED)
                .Length(3).WithMessage(Constants.ValidationMessages.TO_CURRENCY_LEN);

            RuleFor(t => t)
                .Must(t => t.FromCurrency != t.ToCurrency).WithMessage(Constants.ValidationMessages.CONVERSION_CURRENCIES_MUST_DIFFER);

            RuleFor(t => t.Value)
                .GreaterThan(0).WithMessage(Constants.ValidationMessages.INVALID_VALUE);
        }
    }
}
