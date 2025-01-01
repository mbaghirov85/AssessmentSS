using AssessmentPlatformDeveloper.Models;
using AssessmentPlatformDeveloper.Repositories;
using System;
using System.Collections.Generic;

namespace AssessmentPlatformDeveloper.Services {

    public interface ICustomerService {

        IEnumerable<Customer> GetAllCustomers();

        Customer GetCustomer(int id);

        void AddCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void DeleteCustomer(int id);
    }

    public class CustomerService : ICustomerService {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository customerRepository) {
            this.customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomers() {
            return customerRepository.GetAll();
        }

        public Customer GetCustomer(int id) {
            // ensure that customer exists
            var customer = customerRepository.Get(id);
            if (customer == null) {
                throw new ArgumentException($"Customer with ID {id} does not exist.");
            }
            return customer;
        }

        public void AddCustomer(Customer customer) {
            // checking if customer data was submitted properly
            if (customer == null) {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
            }
            customerRepository.Add(customer);
        }

        public void UpdateCustomer(Customer customer) {
            // ensure that customer exists
            var existingCustomer = customerRepository.Get(customer.ID);
            if (existingCustomer == null) {
                throw new ArgumentException($"Cannot update. Customer with ID {customer.ID} does not exist.");
            }

            customerRepository.Update(customer);
        }

        public void DeleteCustomer(int id) {
            // ensure that cusotmer exists
            var existingCustomer = customerRepository.Get(id);
            if (existingCustomer == null) {
                throw new ArgumentException($"Cannot delete. Customer with ID {id} does not exist.");
            }

            customerRepository.Delete(id);
        }
    }
}