using assessment_platform_developer.Helpers;
using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using Newtonsoft.Json;

namespace assessment_platform_developer.Services {

    public interface ICustomerValidationService {

        ValidationResult ValidateAdd(Customer customer);

        ValidationResult ValidateUpdate(Customer customer);

        ValidationResult ValidateDelete(int ID);

        ValidationResult ValidateHttpAdd(string requestBody);

        ValidationResult ValidateHttpUpdate(int ID, string requestBody);

        ValidationResult ValidateHttpDelete(int ID);
    };

    public class CustomerValidationService : ICustomerValidationService {
        private readonly IPostalCodeValidator _postalCodeValidator;
        private readonly IEmailValidator _emailValidator;
        private readonly ICustomerRepository _repository;

        public CustomerValidationService(ICustomerRepository repository) {
            this._repository = repository;
            this._postalCodeValidator = new PostalCodeValidator();
            this._emailValidator = new EmailValidator();
        }

        public ValidationResult ValidateHttpAdd(string requestBody) {
            if (string.IsNullOrEmpty(requestBody))
                return ValidationResult.Failure("customer data must be provided");

            try {
                var customer = JsonConvert.DeserializeObject<Customer>(requestBody);
            } catch (JsonReaderException) {
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
            } catch (JsonReaderException) {
                return ValidationResult.Failure("Invalid JSON format");
            }

            return ValidationResult.Success;
        }

        public ValidationResult ValidateHttpDelete(int ID) {
            if (ID == 0)
                return ValidationResult.Failure("Invalid customer ID");

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

            if (_repository.IsPhoneOrEmailTaken(customer.ID, customer.Phone, customer.Email))
                return ValidationResult.Failure("Phone number or email already exists in the database");

            return ValidationResult.Success;
        }

        public ValidationResult ValidateUpdate(Customer customer) {
            var existing = _repository.Get(customer.ID);
            if (existing == null)
                return ValidationResult.Failure($"Not found");

            return ValidateAdd(customer);
        }

        public ValidationResult ValidateDelete(int ID) {
            var existing = _repository.Get(ID);
            if (existing == null)
                return ValidationResult.Failure($"Not found");

            return ValidationResult.Success;
        }
    }
}