using FluentValidation.Results;
using System;

namespace Currencies.Api.Validation
{
    public static class ValidationExtensions
    {
        public static string ReadValidationErrors(this ValidationResult validation)
            => String.Join(" - ", validation.Errors);
    }
}
