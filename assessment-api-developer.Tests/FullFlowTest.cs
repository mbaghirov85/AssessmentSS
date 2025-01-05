using Microsoft.VisualStudio.TestTools.UnitTesting;
using assessment_platform_developer.Controllers;
using assessment_platform_developer.Models;
using assessment_platform_developer.Services;
using Moq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace assessment_platform_developer.Tests {

    [TestClass]
    public class CFullFlowTest {
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
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();
        }

        [TestMethod]
        public async Task FullCustomerFlow_SuccessfulOperations() {
            // Initial customer data
            var customer = new Customer {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890"
            };

            // 1. Add
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            var jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addResult = await _controller.AddCustomer();
            Assert.IsInstanceOfType(addResult, typeof(CreatedNegotiatedContentResult<ValidationResult>));

            // Simulate the added customer with ID
            customer.ID = 1;

            // 2. Get
            _mockCustomerGetService.Setup(s => s.GetCustomer(1)).Returns(customer);
            var getResult1 = _controller.GetCustomer(1);
            Assert.IsInstanceOfType(getResult1, typeof(OkNegotiatedContentResult<Customer>));

            // 3. Update
            customer.Name = "John Updated";
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.UpdateCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var updateResult1 = await _controller.UpdateCustomer(1);
            Assert.IsInstanceOfType(updateResult1, typeof(StatusCodeResult));

            // 4. Get
            _mockCustomerGetService.Setup(s => s.GetCustomer(1)).Returns(customer);
            var getResult2 = _controller.GetCustomer(1);
            Assert.IsInstanceOfType(getResult2, typeof(OkNegotiatedContentResult<Customer>));

            // 5. Update
            customer.Email = "john.updated@example.com";
            jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var updateResult2 = await _controller.UpdateCustomer(1);
            Assert.IsInstanceOfType(updateResult2, typeof(StatusCodeResult));

            // 6. Get
            _mockCustomerGetService.Setup(s => s.GetCustomer(1)).Returns(customer);
            var getResult3 = _controller.GetCustomer(1);
            Assert.IsInstanceOfType(getResult3, typeof(OkNegotiatedContentResult<Customer>));

            // 7. Update
            customer.Phone = "9876543210";
            jsonContent = JsonConvert.SerializeObject(customer);
            _controller.Request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var updateResult3 = await _controller.UpdateCustomer(1);
            Assert.IsInstanceOfType(updateResult3, typeof(StatusCodeResult));

            // 8. Get
            _mockCustomerGetService.Setup(s => s.GetCustomer(1)).Returns(customer);
            var getResult4 = _controller.GetCustomer(1);
            Assert.IsInstanceOfType(getResult4, typeof(OkNegotiatedContentResult<Customer>));

            // 9. Delete
            _mockCustomerValidationService.Setup(s => s.ValidateHttpDelete(It.IsAny<int>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.DeleteCustomer(It.IsAny<int>())).Returns(ValidationResult.Success);
            var deleteResult = _controller.DeleteCustomer(1);
            Assert.IsInstanceOfType(deleteResult, typeof(StatusCodeResult));

            // 10. Get (should return NotFound)
            _mockCustomerGetService.Setup(s => s.GetCustomer(1)).Returns((Customer)null);
            var getResult5 = _controller.GetCustomer(1);
            Assert.IsInstanceOfType(getResult5, typeof(NotFoundResult));
        }
    }
}