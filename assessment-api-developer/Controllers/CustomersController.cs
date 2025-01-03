using AssessmentPlatformDeveloper.Services;
using AssessmentPlatformDeveloper.Models;
using System.Collections.Generic;
using System.Web.Http;
using System;
using AssessmentPlatformDeveloper.Repositories;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace AssessmentPlatformDeveloper.Controllers {

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
        public IHttpActionResult AddCustomer() {
            var rawBody = Request.Content.ReadAsStringAsync().Result;
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
        public IHttpActionResult UpdateCustomer(int ID) {
            var rawBody = Request.Content.ReadAsStringAsync().Result;
            System.Diagnostics.Debug.WriteLine($"Controller.UpdateCustomer==>{rawBody}");
            try {
                Customer customer = JsonConvert.DeserializeObject<Customer>(rawBody);
                if (customer == null) {
                    return BadRequest("customer data must be provided");
                }

                if (ID != customer.ID) {
                    return BadRequest("Customer IDs does not match");
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