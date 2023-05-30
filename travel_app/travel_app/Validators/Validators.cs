using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace travel_app.Validators
{
    internal class Validators
    {
    }

    public class NotEmptyValidationRule : ValidationRule
    {
        public string Message { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class NotEmptyDateValidationRule : ValidationRule
    {
        public string Message { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime? date = value as DateTime?;
            if (date == null)
            {
                return new ValidationResult(false, Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class NoPastDateValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.Today)
                {
                    return new ValidationResult(false, Message);
                }
            }
            return ValidationResult.ValidResult;
        }
    }

    public class PositiveIntegerValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string text)
            {
                if (int.TryParse(text, out int number))
                {
                    if (number > 0)
                    {
                        return ValidationResult.ValidResult;
                    }
                }
            }
            return new ValidationResult(false, Message);
        }
    }

    public class NotEmptyComboBoxValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, Message);
            }
            return ValidationResult.ValidResult;
        }
    }
}
