using assessment_platform_developer.Helpers;
using assessment_platform_developer.Models;

namespace assessment_platform_developer.Services {

    public interface ICustomerValidationService {

        ValidationResult ValidateCustomer(Customer customer);
    };

    public class CustomerValidationService : ICustomerValidationService {
        private readonly IPostalCodeValidator _postalCodeValidator = new PostalCodeValidator();
        private readonly IEmailValidator _emailValidator = new EmailValidator();

        public ValidationResult ValidateCustomer(Customer customer) {
            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.Phone))
                return ValidationResult.Failure("Mandatory fields missing");

            if (!_emailValidator.IsValidEmail(customer.Email))
                return ValidationResult.Failure("Invalid email format");

            if (!string.IsNullOrEmpty(customer.ContactEmail) && !_emailValidator.IsValidEmail(customer.ContactEmail))
                return ValidationResult.Failure("Invalid contact email format");

            if (!string.IsNullOrEmpty(customer.Zip) && !_postalCodeValidator.Validate(customer.Country, customer.Zip))
                return ValidationResult.Failure("Invalid postal code");

            return ValidationResult.Success;
        }
    }
}