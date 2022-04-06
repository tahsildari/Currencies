using System;
using FluentValidation.Results;

namespace Currencies.Api.Validation
{
    public static class ValidationExtensions
    {
        public static string ReadValidationErrors(this ValidationResult validation)
            => String.Join(" - ", validation.Errors);
    }
}
