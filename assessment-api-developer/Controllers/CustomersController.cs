using assessment_platform_developer.Services;
using assessment_platform_developer.Models;
using System.Web.Http;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebGrease.Extensions;

namespace assessment_platform_developer.Controllers {

    /// <summary>
    /// API Controller for managing Customer operations.
    /// Provides endpoints for CRUD operations on customers.
    /// </summary>
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerService">The customer service dependency.</param>
        // GET: api/customers
        public CustomersController(ICustomerService customerService) {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>HTTP response containing a list of customers.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllCustomers() {
            var customers = _customerService.GetAllCustomers();
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
            var customer = _customerService.GetCustomer(ID);
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
                var existingCustomer = _customerService.GetCustomer(ID);
                if (existingCustomer == null) {
                    return NotFound();
                }

                _customerService.DeleteCustomer(ID);
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
            rawBody.EnsureEndSeparator();
            try {
                Customer customer = JsonConvert.DeserializeObject<Customer>(rawBody);
                if (customer == null) {
                    return BadRequest("customer data must be provided");
                }
                _customerService.AddCustomer(customer);
                return Created($"api/customers/{customer.ID}", customer);
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
            rawBody.EnsureEndSeparator();
            try {
                Customer customer = JsonConvert.DeserializeObject<Customer>(rawBody);
                if (customer == null) {
                    return BadRequest("customer data must be provided");
                }

                if (ID != customer.ID) {
                    return BadRequest("Customer IDs provided in the URL and Body does not match");
                }

                var existingCustomer = _customerService.GetCustomer(customer.ID);
                if (existingCustomer == null) {
                    return NotFound();
                }

                _customerService.UpdateCustomer(customer);
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            } catch (ArgumentNullException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}