using System.Collections.Generic;
using System.Text.RegularExpressions;
using AssessmentPlatformDeveloper.Models;

namespace AssessmentPlatformDeveloper.Helpers {

    public class PostalCodeValidator {

        private static readonly Dictionary<string, string> _postalCodePatterns = new Dictionary<string, string>{
            { EnumExtensions.GetEnumDescription(Countries.Canada), @"^[A-Z]\d[A-Z] \d[A-Z]\d$" },
            { EnumExtensions.GetEnumDescription(Countries.UnitedStates), @"^\d{5}(-\d{4})?$" }
        };

        public static bool Validate(string country, string postalCode) {
            return Regex.IsMatch(postalCode, GetValidationExpression(country));
        }

        public static string GetValidationExpression(string country) {
            return _postalCodePatterns.TryGetValue(country, out var expression) ? expression : string.Empty;
        }
    }
}