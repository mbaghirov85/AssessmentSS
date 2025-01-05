using Microsoft.VisualStudio.TestTools.UnitTesting;
using assessment_platform_developer.Services;
using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace assessment_platform_developer.Tests {

    [TestClass]
    public class GetCustomerByIdTests {
        private ICustomerGetService _customerGetService;
        private ICustomerRepository _customerRepository;

        [TestInitialize]
        public void TestInitialize() {
            _customerRepository = new CustomerRepository();
            _customerGetService = new CustomerGetService(_customerRepository);
        }

        [TestMethod]
        public void GetAllCustomers_EmptyDatabase_ReturnsEmptyList() {
            // Arrange
            // Database is already empty due to fresh initialization

            // Act
            var result = _customerGetService.GetAllCustomers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetAllCustomers_MultipleCustomers_ReturnsAllCustomers() {
            // Arrange
            var customers = new List<Customer>
            {
            new Customer { ID = 1, Name = "John Doe", Email = "john@example.com", Phone = "1234567890" },
            new Customer { ID = 2, Name = "Jane Smith", Email = "jane@example.com", Phone = "0987654321" }
        };
            foreach (var customer in customers) {
                _customerRepository.Add(customer);
            }

            // Act
            var result = _customerGetService.GetAllCustomers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customers.Count, result.Count());
            CollectionAssert.AreEquivalent(customers, result.ToList());
        }

        [TestMethod]
        public void GetAllCustomers_VerifyCorrectNumberReturned() {
            // Arrange
            var expectedCount = 3;
            for (int i = 1; i <= expectedCount; i++) {
                _customerRepository.Add(new Customer { ID = i, Name = $"Customer {i}", Email = $"customer{i}@example.com", Phone = $"123456789{i}" });
            }

            // Act
            var result = _customerGetService.GetAllCustomers();

            // Assert
            Assert.AreEqual(expectedCount, result.Count());
        }

        [TestMethod]
        public void GetAllCustomers_CheckRequiredFields() {
            // Arrange
            var customer = new Customer {
                ID = 1,
                Name = "Test Customer",
                Email = "test@example.com",
                Phone = "1234567890",
                Address = "123 Test St",
                City = "Test City",
                State = "TS",
                Zip = "12345",
                Country = "Test Country"
            };
            _customerRepository.Add(customer);

            // Act
            var result = _customerGetService.GetAllCustomers().FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.ID, result.ID);
            Assert.AreEqual(customer.Name, result.Name);
            Assert.AreEqual(customer.Email, result.Email);
            Assert.AreEqual(customer.Phone, result.Phone);
            Assert.AreEqual(customer.Address, result.Address);
            Assert.AreEqual(customer.City, result.City);
            Assert.AreEqual(customer.State, result.State);
            Assert.AreEqual(customer.Zip, result.Zip);
            Assert.AreEqual(customer.Country, result.Country);
        }
    }
}