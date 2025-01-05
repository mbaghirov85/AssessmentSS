using Microsoft.VisualStudio.TestTools.UnitTesting;
using assessment_platform_developer.Controllers;
using assessment_platform_developer.Models;
using assessment_platform_developer.Services;
using Moq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Threading.Tasks;
using System;
using assessment_platform_developer.Repositories;

namespace assessment_platform_developer.Tests {

    [TestClass]
    public class PostCustomerTests {
        private ICustomerRepository _customerRepository;
        private CustomersController _controller;
        private ICustomerGetService _customerGetService;
        private Mock<ICustomerGetService> _mockCustomerGetService;
        private Mock<ICustomerManageService> _mockCustomerManageService;
        private Mock<ICustomerValidationService> _mockCustomerValidationService;

        [TestInitialize]
        public void TestInitialize() {
            _customerRepository = new CustomerRepository();
            _customerGetService = new CustomerGetService(_customerRepository);
            _mockCustomerGetService = new Mock<ICustomerGetService>();
            _mockCustomerManageService = new Mock<ICustomerManageService>();
            _mockCustomerValidationService = new Mock<ICustomerValidationService>();
            _controller = new CustomersController(
                _mockCustomerGetService.Object,
                _mockCustomerManageService.Object,
                _mockCustomerValidationService.Object
            );
        }

        [TestMethod]
        public async Task AddCustomer_WithAllRequiredFields_ReturnsCreatedResult() {
            // Arrange
            var customer = new Customer {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890"
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<ValidationResult>));
        }

        [TestMethod]
        public async Task AddCustomer_WithMissingRequiredFields_ReturnsBadRequest() {
            // Arrange
            var customer = new Customer {
                Name = "John Doe"
                // Missing Email and Phone
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Failure("Mandatory fields missing"));

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddCustomer_WithAllFields_ReturnsCreatedResult() {
            // Arrange
            var customer = new Customer {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                Zip = "12345",
                Country = "United States",
                Notes = "Test notes",
                ContactName = "Jane Doe",
                ContactPhone = "0987654321",
                ContactEmail = "jane@example.com",
                ContactTitle = "Manager",
                ContactNotes = "Contact notes"
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<ValidationResult>));
        }

        [TestMethod]
        public async Task AddCustomer_WithInvalidEmailFormat_ReturnsBadRequest() {
            // Arrange
            var customer = new Customer {
                Name = "John Doe",
                Email = "invalid-email",
                Phone = "1234567890"
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Failure("Invalid email format"));

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddCustomer_WithInvalidPostalCode_ReturnsBadRequest() {
            // Arrange
            var customer = new Customer {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Country = "United States",
                Zip = "invalid-zip"
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Failure("Invalid postal code"));

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddCustomer_WithMaxLengthFields_ReturnsCreatedResult() {
            // Arrange
            var customer = new Customer {
                Name = new string('A', 100),
                Email = new string('a', 50) + "@example.com",
                Phone = new string('1', 20),
                Address = new string('B', 200),
                City = new string('C', 50),
                State = new string('D', 20),
                Zip = new string('1', 10),
                Country = new string('E', 50),
                Notes = new string('F', 500),
                ContactName = new string('G', 100),
                ContactPhone = new string('2', 20),
                ContactEmail = new string('b', 50) + "@example.com",
                ContactTitle = new string('H', 50),
                ContactNotes = new string('I', 500)
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<ValidationResult>));
        }

        [TestMethod]
        public async Task AddCustomer_DuplicateCustomer_ReturnsBadRequest() {
            // Arrange
            var customer = new Customer {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890"
            };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            var _res = _customerGetService.GetAllCustomers();
            foreach (var item in _res) {
                Console.WriteLine("ID: " + item.ID + " Name: " + item.Name + " Email: " + item.Email + " Phone: " + item.Phone);
            }
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Failure("Customer already exists"));

            // Act
            var result = await _controller.AddCustomer();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }
    }
}