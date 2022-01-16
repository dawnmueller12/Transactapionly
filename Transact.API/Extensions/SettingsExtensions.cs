using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transact.API.Extensions
{
    public static class SettingsExtensions
    {
        public static List<string> IsValid<T>(this T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            List<string> errors = new List<string>();
            var validationResult = new List<ValidationResult>();
            var result = Validator.TryValidateObject(data, new ValidationContext(data), validationResult, false);

            if (!result)
            {
                foreach (var item in validationResult)
                {
                    errors.Add($"ERROR::{item.MemberNames}:{item.ErrorMessage}");
                }
            }

            return errors;
        }
    }
}
