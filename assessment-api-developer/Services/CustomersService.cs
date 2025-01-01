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

            /* // Here we can add double check if parameters are sent from the web form and only in that case update the values in the storage
            if (!string.IsNullOrEmpty(customer.Name))
                existingCustomer.Name = customer.Name;

            if (!string.IsNullOrEmpty(customer.Address))
                existingCustomer.Address = customer.Address;

            if (!string.IsNullOrEmpty(customer.Email))
                existingCustomer.Email = customer.Email;

            if (!string.IsNullOrEmpty(customer.Phone))
                existingCustomer.Phone = customer.Phone;

            if (!string.IsNullOrEmpty(customer.City))
                existingCustomer.City = customer.City;

            if (!string.IsNullOrEmpty(customer.State))
                existingCustomer.State = customer.State;

            if (!string.IsNullOrEmpty(customer.Zip))
                existingCustomer.Zip = customer.Zip;

            if (!string.IsNullOrEmpty(customer.Country))
                existingCustomer.Country = customer.Country;

            if (!string.IsNullOrEmpty(customer.Notes))
                existingCustomer.Notes = customer.Notes;

            if (!string.IsNullOrEmpty(customer.ContactName))
                existingCustomer.ContactName = customer.ContactName;

            if (!string.IsNullOrEmpty(customer.ContactPhone))
                existingCustomer.ContactPhone = customer.ContactPhone;

            if (!string.IsNullOrEmpty(customer.ContactEmail))
                existingCustomer.ContactEmail = customer.ContactEmail;

            if (!string.IsNullOrEmpty(customer.ContactTitle))
                existingCustomer.ContactTitle = customer.ContactTitle;

            if (!string.IsNullOrEmpty(customer.ContactNotes))
                existingCustomer.ContactNotes = customer.ContactNotes;

            customerRepository.Update(existingCustomer);

            // if uncommiting this block then remove the next line which updates the customer repository
            */

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