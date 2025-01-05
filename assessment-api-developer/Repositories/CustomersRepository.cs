using assessment_platform_developer.Models;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace assessment_platform_developer.Repositories {

    public interface ICustomerRepository {

        IEnumerable<Customer> GetAll();

        Customer Get(int id);

        void Add(Customer customer);

        void Update(Customer customer);

        void Delete(int id);

        bool IsPhoneOrEmailTaken(int ID, string phone, string email);
    }

    public class CustomerRepository : ICustomerRepository {

        // Assuming you have a DbContext named 'context'
        private readonly List<Customer> customers = new List<Customer>();

        public IEnumerable<Customer> GetAll() {
            return customers;
        }

        public Customer Get(int id) {
            return customers.FirstOrDefault(c => c.ID == id);
        }

        public void Add(Customer customer) {
            if (customer.ID == 0) {
                customer.ID = customers.Any() ? customers.Maxima(c => c.ID).First().ID + 1 : 1;
            }
            customers.Add(customer);
        }

        public void Update(Customer customer) {
            var existingCustomer = customers.FirstOrDefault(c => c.ID == customer.ID);
            if (existingCustomer != null) {
                existingCustomer.Name = customer.Name;
                existingCustomer.Address = customer.Address;
                existingCustomer.Email = customer.Email;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.City = customer.City;
                existingCustomer.State = customer.State;
                existingCustomer.Zip = customer.Zip;
                existingCustomer.Country = customer.Country;
                existingCustomer.Notes = customer.Notes;
                existingCustomer.ContactName = customer.ContactName;
                existingCustomer.ContactPhone = customer.ContactPhone;
                existingCustomer.ContactEmail = customer.ContactEmail;
                existingCustomer.ContactTitle = customer.ContactTitle;
                existingCustomer.ContactNotes = customer.ContactNotes;
            }
        }

        public void Delete(int id) {
            var customer = customers.FirstOrDefault(c => c.ID == id);
            if (customer != null) {
                customers.Remove(customer);
            }
        }

        public bool IsPhoneOrEmailTaken(int ID, string phone, string email) {
            return customers.Any(c => (ID == 0 || c.ID != ID) && (c.Phone == phone || c.Email == email));
        }
    }
}