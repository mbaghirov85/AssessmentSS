using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using System.Collections.Generic;

namespace assessment_platform_developer.Services {

    public interface ICustomerServiceGet {

        IEnumerable<Customer> GetAllCustomers();

        Customer GetCustomer(int id);
    }

    public class CustomerServiceGet : ICustomerServiceGet {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerValidationService _validator;

        public CustomerServiceGet(ICustomerRepository customerRepository, ICustomerValidationService validator) {
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
    }
}