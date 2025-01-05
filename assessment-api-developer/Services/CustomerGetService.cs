using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using System.Collections.Generic;

namespace assessment_platform_developer.Services {

    public interface ICustomerGetService {

        IEnumerable<Customer> GetAllCustomers();

        Customer GetCustomer(int id);
    }

    public class CustomerGetService : ICustomerGetService {
        private readonly ICustomerRepository _customerRepository;

        public CustomerGetService(ICustomerRepository customerRepository) {
            this._customerRepository = customerRepository;
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