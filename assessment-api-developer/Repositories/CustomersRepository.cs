using assessment_platform_developer.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace assessment_platform_developer.Repositories
{
	public interface ICustomerRepository
	{
		IEnumerable<Customer> GetAll();
		Customer Get(int id);
		void Add(Customer customer);
		void Update(Customer customer);
		void Delete(int id);
	}

	public class CustomerRepository : ICustomerRepository
	{
		// Assuming you have a DbContext named 'context'
		private readonly List<Customer> customers = new List<Customer>();

		public IEnumerable<Customer> GetAll()
		{
			return customers;
		}

		public Customer Get(int id)
		{
			return customers.FirstOrDefault(c => c.ID == id);
		}

		public void Add(Customer customer)
		{
			customers.Add(customer);
		}

		public void Update(Customer customer)
		{
			var existingCustomer = customers.FirstOrDefault(c => c.ID == customer.ID);
			if (existingCustomer != null)
			{
				// Update properties of existingCustomer based on the properties of customer
				// For example:
				existingCustomer.Name = customer.Name;
				// Repeat for other properties
			}
		}

		public void Delete(int id)
		{
			var customer = customers.FirstOrDefault(c => c.ID == id);
			if (customer != null)
			{
				customers.Remove(customer);
			}
		}
	}
}