using Currencies.Common;
using FluentValidation;

namespace Currencies.Api.Validation
{
    public class HistoryValidation : AbstractValidator<int>
    {
        public HistoryValidation()
        {
            RuleFor(t => t)
                .NotEqual(0).WithMessage(Constants.ValidationMessages.HISTORY_CANNOT_BE_ZERO)
                .GreaterThanOrEqualTo(1).WithMessage(Constants.ValidationMessages.HISTORY_MUST_BE_POSITIVE);
        }
    }
}
