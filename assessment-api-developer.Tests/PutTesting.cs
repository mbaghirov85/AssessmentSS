using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using assessment_platform_developer.Controllers;
using assessment_platform_developer.Models;
using assessment_platform_developer.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace assessment_platform_developer.Tests {

    [TestClass]
    public class PutTesting {
        private CustomersController _controller;
        private Mock<ICustomerGetService> _mockCustomerGetService;
        private Mock<ICustomerManageService> _mockCustomerManageService;
        private Mock<ICustomerValidationService> _mockCustomerValidationService;

        [TestInitialize]
        public void TestInitialize() {
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
        public async Task UpdateCustomer_WithValidData_ReturnsNoContent() {
            // Arrange
            int customerId = 1;
            var customer = new Customer { ID = customerId, Name = "John Doe", Email = "john@example.com", Phone = "1234567890" };
            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request = new HttpRequestMessage(HttpMethod.Put, $"api/customers/{customerId}");
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(customerId, It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.UpdateCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            var result = await _controller.UpdateCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_NonExistentCustomer_ReturnsNotFound() {
            // Arrange
            int customerId = 999;
            var customer = new Customer { ID = customerId, Name = "John Doe", Email = "john@example.com", Phone = "1234567890" };
            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request = new HttpRequestMessage(HttpMethod.Put, $"api/customers/{customerId}");
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(customerId, It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.UpdateCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Failure("Not found"));

            // Act
            var result = await _controller.UpdateCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateCustomer_SpecificFields_ReturnsNoContent() {
            // Arrange
            int customerId = 1;
            var customer = new Customer { ID = customerId, Name = "John Updated", Email = "john@example.com", Phone = "1234567890" };
            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request = new HttpRequestMessage(HttpMethod.Put, $"api/customers/{customerId}");
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(customerId, It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.UpdateCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            var result = await _controller.UpdateCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_InvalidData_ReturnsBadRequest() {
            // Arrange
            int customerId = 1;
            var customer = new Customer { ID = customerId, Name = "John Doe", Email = "invalid-email", Phone = "1234567890" };
            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request = new HttpRequestMessage(HttpMethod.Put, $"api/customers/{customerId}");
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(customerId, It.IsAny<string>())).Returns(ValidationResult.Failure("Invalid email format"));

            // Act
            var result = await _controller.UpdateCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task UpdateCustomer_ChangeCountry_ValidatesPostalCode() {
            // Arrange
            int customerId = 1;
            var customer = new Customer { ID = customerId, Name = "John Doe", Email = "john@example.com", Phone = "1234567890", Country = "Canada", Zip = "A1A 1A1" };
            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request = new HttpRequestMessage(HttpMethod.Put, $"api/customers/{customerId}");
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(customerId, It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.UpdateCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            var result = await _controller.UpdateCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
            _mockCustomerValidationService.Verify(s => s.ValidateHttpUpdate(customerId, It.Is<string>(str => str.Contains("\"Country\":\"Canada\"") && str.Contains("\"Zip\":\"A1A 1A1\""))), Times.Once);
        }

        [TestMethod]
        public async Task UpdateCustomer_IdMismatch_ReturnsBadRequest() {
            // Arrange
            int urlId = 1;
            int bodyId = 2;
            var customer = new Customer { ID = bodyId, Name = "John Doe", Email = "john@example.com", Phone = "1234567890" };
            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request = new HttpRequestMessage(HttpMethod.Put, $"api/customers/{urlId}");
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(urlId, It.IsAny<string>())).Returns(ValidationResult.Failure("ID in URL does not match ID in request body"));

            // Act
            var result = await _controller.UpdateCustomer(urlId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("customer data must be provided", badRequestResult.Message);
        }
    }
}