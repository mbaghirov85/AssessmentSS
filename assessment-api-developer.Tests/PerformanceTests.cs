using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using assessment_platform_developer.Controllers;
using assessment_platform_developer.Models;
using assessment_platform_developer.Services;
using Moq;
using System.Web.Http.Results;

namespace assessment_platform_developer.Tests {

    [TestClass]
    public class PerformanceTests {
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
        public void TestRetrieveLargeNumberOfCustomers() {
            // Arrange
            const int totalCustomers = 10000;
            var customers = Enumerable.Range(1, totalCustomers)
                .Select(i => new Customer { ID = i, Name = $"Customer {i}" })
                .ToList();
            _mockCustomerGetService.Setup(s => s.GetAllCustomers()).Returns(customers);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = _controller.GetAllCustomers() as OkNegotiatedContentResult<IEnumerable<Customer>>;
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(totalCustomers, result.Content.Count());
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000, "Retrieving customers took too long");
        }

        [TestMethod]
        public async Task TestConcurrentCustomerOperations() {
            // Arrange
            const int concurrentOperations = 100;
            var tasks = new List<Task>();
            var customer = new Customer { Name = "Test Customer", Email = "test@example.com", Phone = "1234567890" };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);

            // Act
            for (int i = 0; i < concurrentOperations; i++) {
                tasks.Add(_controller.AddCustomer());
            }
            await Task.WhenAll(tasks);

            // Assert
            _mockCustomerManageService.Verify(s => s.AddCustomer(It.IsAny<Customer>()), Times.Exactly(concurrentOperations));
        }

        [TestMethod]
        public async Task MeasureResponseTimesUnderLoad() {
            // Arrange
            const int iterations = 1000;
            var customer = new Customer { ID = 1, Name = "Test Customer", Email = "test@example.com", Phone = "1234567890" };
            _mockCustomerValidationService.Setup(s => s.ValidateHttpAdd(It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);
            _mockCustomerValidationService.Setup(s => s.ValidateHttpUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.UpdateCustomer(It.IsAny<Customer>())).Returns(ValidationResult.Success);
            _mockCustomerValidationService.Setup(s => s.ValidateHttpDelete(It.IsAny<int>())).Returns(ValidationResult.Success);
            _mockCustomerManageService.Setup(s => s.DeleteCustomer(It.IsAny<int>())).Returns(ValidationResult.Success);

            // Act
            var addStopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++) {
                await _controller.AddCustomer();
            }
            addStopwatch.Stop();

            var updateStopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++) {
                await _controller.UpdateCustomer(1);
            }
            updateStopwatch.Stop();

            var deleteStopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++) {
                _controller.DeleteCustomer(1);
            }
            deleteStopwatch.Stop();

            // Assert
            Assert.IsTrue(addStopwatch.ElapsedMilliseconds / iterations < 10, "Adding customers took too long on average");
            Assert.IsTrue(updateStopwatch.ElapsedMilliseconds / iterations < 10, "Updating customers took too long on average");
            Assert.IsTrue(deleteStopwatch.ElapsedMilliseconds / iterations < 10, "Deleting customers took too long on average");
        }
    }
}