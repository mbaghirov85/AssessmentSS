using assessment_platform_developer.Helpers;
using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using Newtonsoft.Json;

namespace assessment_platform_developer.Services {

    public interface ICustomerValidationService {

        ValidationResult ValidateAdd(Customer customer);

        ValidationResult ValidateUpdate(Customer customer);

        ValidationResult ValidateHttpAdd(string requestBody);

        ValidationResult ValidateHttpUpdate(int ID, string requestBody);
    };

    public class CustomerValidationService : ICustomerValidationService {
        private readonly IPostalCodeValidator _postalCodeValidator = new PostalCodeValidator();
        private readonly IEmailValidator _emailValidator = new EmailValidator();
        private readonly ICustomerRepository _repository;

        public CustomerValidationService(ICustomerRepository repository) {
            this._repository = repository;
        }

        public ValidationResult ValidateHttpAdd(string requestBody) {
            if (string.IsNullOrEmpty(requestBody))
                return ValidationResult.Failure("customer data must be provided");

            try {
                var customer = JsonConvert.DeserializeObject<Customer>(requestBody);
            } catch (JsonReaderException ex) {
                return ValidationResult.Failure("Invalid JSON format");
            }

            return ValidationResult.Success;
        }

        public ValidationResult ValidateHttpUpdate(int ID, string requestBody) {
            if (string.IsNullOrEmpty(requestBody))
                return ValidationResult.Failure("customer data must be provided");

            try {
                var customer = JsonConvert.DeserializeObject<Customer>(requestBody);

                if (ID != customer.ID)
                    return ValidationResult.Failure("ID in URL does not match ID in request body");
            } catch (JsonReaderException ex) {
                return ValidationResult.Failure("Invalid JSON format");
            }

            return ValidationResult.Success;
        }

        public ValidationResult ValidateAdd(Customer customer) {
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

        public ValidationResult ValidateUpdate(Customer customer) {
            var existing = _repository.Get(customer.ID);
            if (existing == null)
                return ValidationResult.Failure($"Not found");

            return ValidateAdd(customer);
        }
    }
}