using System.Collections.Generic;
using System.Text.RegularExpressions;
using assessment_platform_developer.Models;

namespace assessment_platform_developer.Helpers {

    public interface IPostalCodeValidator {

        bool Validate(string country, string postalCode);

        string GetValidationExpression(string country);
    }

    public class PostalCodeValidator : IPostalCodeValidator {

        private static readonly Dictionary<string, string> _postalCodePatterns = new Dictionary<string, string>{
            { EnumExtensions.GetEnumDescription(Countries.Canada), @"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$" },
            { EnumExtensions.GetEnumDescription(Countries.UnitedStates), @"^\d{5}(-\d{4})?$" }
        };

        public bool Validate(string country, string postalCode) {
            return Regex.IsMatch(postalCode, GetValidationExpression(country));
        }

        public string GetValidationExpression(string country) {
            return _postalCodePatterns.TryGetValue(country, out var expression) ? expression : string.Empty;
        }
    }
}