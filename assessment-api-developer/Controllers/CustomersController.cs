using assessment_platform_developer.Services;
using assessment_platform_developer.Models;
using System.Web.Http;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace assessment_platform_developer.Controllers {

    /// <summary>
    /// API Controller for managing Customer operations.
    /// Provides endpoints for CRUD operations on customers.
    /// </summary>
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController {
        private readonly ICustomerGetService _customerGetService;
        private readonly ICustomerManageService _customerManageService;
        private readonly ICustomerValidationService _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerService">The customer service dependency.</param>
        // GET: api/customers
        public CustomersController(ICustomerGetService customerGetService, ICustomerManageService customerManageService, ICustomerValidationService validator) {
            this._customerGetService = customerGetService ?? throw new ArgumentNullException(nameof(customerGetService));
            this._customerManageService = customerManageService ?? throw new ArgumentNullException(nameof(customerManageService));
            this._validator = validator;
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>HTTP response containing a list of customers.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllCustomers() {
            var customers = _customerGetService.GetAllCustomers();
            return Ok(customers);
        }

        /// <summary>
        /// Retrieves a specific customer by ID.
        /// </summary>
        /// <param name="ID">The unique identifier of the customer.</param>
        /// <returns>HTTP response containing the customer data or a NotFound response if not found.</returns>
        [HttpGet]
        [Route("{ID:int}")]
        public IHttpActionResult GetCustomer(int ID) {
            var customer = _customerGetService.GetCustomer(ID);
            if (customer == null) {
                return NotFound();
            }
            return Ok(customer);
        }

        /// <summary>
        /// Deletes a specific customer by ID.
        /// </summary>
        /// <param name="ID">The unique identifier of the customer to delete.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        [HttpDelete]
        [Route("{ID:int}")]
        public IHttpActionResult DeleteCustomer(int ID) {
            try {
                ValidationResult validationResult = _validator.ValidateHttpDelete(ID);
                if (!validationResult.IsValid) {
                    return BadRequest(validationResult.ErrorMessage);
                }

                var result = _customerManageService.DeleteCustomer(ID);
                if (!result.IsValid) {
                    if (result.ErrorMessage == "Not found") {
                        return NotFound();
                    }
                    return BadRequest(result.ErrorMessage);
                }
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <returns>HTTP response containing the created customer's data or an error message.</returns>
        /// <remarks>
        /// IN->Body = json Customer object
        /// </remarks>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddCustomer() {
            var rawBody = await Request.Content.ReadAsStringAsync();
            try {
                var validationResult = _validator.ValidateHttpAdd(rawBody);
                if (!validationResult.IsValid) {
                    return BadRequest(validationResult.ErrorMessage);
                }

                Customer customer = JsonConvert.DeserializeObject<Customer>(rawBody);

                var result = _customerManageService.AddCustomer(customer);
                if (!result.IsValid) {
                    return BadRequest(result.ErrorMessage);
                }
                return Created("ok", result);
            } catch (JsonException ex) {
                return BadRequest($"Invalid customer data format: {ex.Message}");
            } catch (ArgumentNullException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Updates an existing customer's details.
        /// </summary>
        /// <param name="ID">The unique identifier of the customer to update.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        /// <remarks>
        /// IN->Body = json Customer object
        /// </remarks>
        [HttpPut]
        [Route("{ID:int}")]
        public async Task<IHttpActionResult> UpdateCustomer(int ID) {
            var rawBody = await Request.Content.ReadAsStringAsync();

            try {
                var validationResult = _validator.ValidateHttpUpdate(ID, rawBody);
                if (!validationResult.IsValid)
                    return BadRequest("customer data must be provided");

                Customer customer = JsonConvert.DeserializeObject<Customer>(rawBody);

                var result = _customerManageService.UpdateCustomer(customer);
                if (!result.IsValid) {
                    if (result.ErrorMessage == "Not found")
                        return NotFound();

                    return BadRequest(result.ErrorMessage);
                }
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            } catch (ArgumentNullException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}