using Microsoft.VisualStudio.TestTools.UnitTesting;
using assessment_platform_developer.Controllers;
using assessment_platform_developer.Services;
using assessment_platform_developer.Models;
using Moq;
using System.Web.Http.Results;

namespace assessment_platform_developer.Tests {

    [TestClass]
    public class DeleteTests {
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
        public void DeleteCustomer_ExistingCustomer_ReturnsNoContent() {
            // Arrange
            int customerId = 1;
            _mockCustomerValidationService.Setup(s => s.ValidateHttpDelete(customerId)).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.DeleteCustomer(customerId)).Returns(ValidationResult.Success);

            // Act
            var result = _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void DeleteCustomer_NonExistentCustomer_ReturnsNotFound() {
            // Arrange
            int customerId = 999;
            _mockCustomerValidationService.Setup(s => s.ValidateHttpDelete(customerId)).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.DeleteCustomer(customerId)).Returns(ValidationResult.Failure("Not found"));

            // Act
            var result = _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteCustomer_VerifyCannotRetrieveAfterward() {
            // Arrange
            int customerId = 1;
            _mockCustomerValidationService.Setup(s => s.ValidateHttpDelete(customerId)).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.DeleteCustomer(customerId)).Returns(ValidationResult.Success);
            _mockCustomerGetService.Setup(s => s.GetCustomer(customerId)).Returns((Customer)null);

            // Act
            var deleteResult = _controller.DeleteCustomer(customerId);
            var getResult = _controller.GetCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(deleteResult, typeof(StatusCodeResult));
            Assert.IsInstanceOfType(getResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteCustomer_InvalidIdFormat_ReturnsBadRequest() {
            // Arrange
            int invalidCustomerId = -1;
            _mockCustomerValidationService.Setup(s => s.ValidateHttpDelete(invalidCustomerId)).Returns(ValidationResult.Failure("Invalid customer ID"));

            // Act
            var result = _controller.DeleteCustomer(invalidCustomerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("Invalid customer ID", badRequestResult.Message);
        }
    }
}