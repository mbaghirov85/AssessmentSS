using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace assessment_platform_developer.Services {

    public interface ICustomerService {

        IEnumerable<Customer> GetAllCustomers();

        Customer GetCustomer(int id);

        ValidationResult AddCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void DeleteCustomer(int id);
    }

    public class CustomerService : ICustomerService {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerValidationService _validator;

        public CustomerService(ICustomerRepository customerRepository, ICustomerValidationService validator) {
            this._customerRepository = customerRepository;
            this._validator = validator;
        }

        public IEnumerable<Customer> GetAllCustomers() {
            return _customerRepository.GetAll();
        }

        public Customer GetCustomer(int id) {
            var customer = _customerRepository.Get(id);
            return customer;
        }

        public ValidationResult AddCustomer(Customer customer) {
            // checking if customer data was submitted properly
            var validationResult = _validator.ValidateCustomer(customer);
            if (!validationResult.IsValid) {
                return ValidationResult.Failure(validationResult.ErrorMessage);
            }
            try {
                _customerRepository.Add(customer);
                return ValidationResult.Success;
            } catch (Exception ex) {
                return ValidationResult.Failure($"Filed to add customer: {ex.Message}");
            }
        }

        public void UpdateCustomer(Customer customer) {
            // ensure that customer exists
            var existingCustomer = _customerRepository.Get(customer.ID) ?? throw new ArgumentException($"Cannot update. Customer with ID {customer.ID} does not exist.");
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(int id) {
            // ensure that cusotmer exists
            var existingCustomer = _customerRepository.Get(id) ?? throw new ArgumentException($"Cannot delete. Customer with ID {id} does not exist.");
            _customerRepository.Delete(id);
        }
    }
}