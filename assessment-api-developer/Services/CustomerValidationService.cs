using assessment_platform_developer.Helpers;
using assessment_platform_developer.Models;

namespace assessment_platform_developer.Services {

    public interface ICustomerValidationService {

        ValidationResult ValidateCustomer(Customer customer);
    };

    public class CustomerValidationService : ICustomerValidationService {

        public ValidationResult ValidateCustomer(Customer customer) {
            if (!customer.IsValid())
                return ValidationResult.Failure("Mandatory fields missing");

            if (!EmailValidator.IsValidEmail(customer.Email))
                return ValidationResult.Failure("Invalid email format");

            if (!string.IsNullOrEmpty(customer.ContactEmail) && !EmailValidator.IsValidEmail(customer.ContactEmail))
                return ValidationResult.Failure("Invalid contact email format");

            if (!string.IsNullOrEmpty(customer.Zip) && !PostalCodeValidator.Validate(customer.Country, customer.Zip))
                return ValidationResult.Failure("Invalid postal code");

            return ValidationResult.Success;
        }
    }
}