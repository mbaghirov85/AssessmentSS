﻿using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using System;

namespace assessment_platform_developer.Services {

    public interface ICustomerManageService {

        ValidationResult AddCustomer(Customer customer);

        ValidationResult UpdateCustomer(Customer customer);

        ValidationResult DeleteCustomer(int id);
    }

    public class CustomerManageService : ICustomerManageService {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerValidationService _validator;

        public CustomerManageService(ICustomerRepository customerRepository, ICustomerValidationService validator) {
            this._customerRepository = customerRepository;
            this._validator = validator;
        }

        public ValidationResult AddCustomer(Customer customer) {
            // checking if customer data was submitted properly
            var validationResult = _validator.ValidateAdd(customer);
            if (!validationResult.IsValid)
                return ValidationResult.Failure(validationResult.ErrorMessage);

            try {
                _customerRepository.Add(customer);
                return ValidationResult.Success;
            } catch (Exception ex) {
                return ValidationResult.Failure($"Failed to add customer: {ex.Message}");
            }
        }

        public ValidationResult UpdateCustomer(Customer customer) {
            // ensure that customer exists
            var validationResult = _validator.ValidateUpdate(customer);
            if (!validationResult.IsValid) {
                return ValidationResult.Failure(validationResult.ErrorMessage);
            }

            _customerRepository.Update(customer);
            return ValidationResult.Success;
        }

        public ValidationResult DeleteCustomer(int id) {
            var validationResult = _validator.ValidateDelete(id);
            if (!validationResult.IsValid) {
                return ValidationResult.Failure(validationResult.ErrorMessage);
            }
            _customerRepository.Delete(id);
            return ValidationResult.Success;
        }
    }
}