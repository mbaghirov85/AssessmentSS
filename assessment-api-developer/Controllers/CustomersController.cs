using assessment_platform_developer.Services;
using assessment_platform_developer.Models;
using System.Web.Http;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebGrease.Extensions;

namespace assessment_platform_developer.Controllers {

    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService) {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        // GET: api/customers
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllCustomers() {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        // GET: api/customers/{ID}
        [HttpGet]
        [Route("{ID:int}")]
        public IHttpActionResult GetCustomer(int ID) {
            var customer = _customerService.GetCustomer(ID);
            if (customer == null) {
                return NotFound();
            }
            return Ok(customer);
        }

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
                return Ok(customer);
            } catch (ArgumentNullException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}