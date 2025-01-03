using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using AssessmentPlatformDeveloper.Models;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace AssessmentPlatformDeveloper.Services {

    public interface IApiCustomerService {

        Task<List<Customer>> GetAllCustomers();

        Task<Customer> GetCustomer(int id);

        Task AddCustomer(Customer customer);

        Task UpdateCustomer(Customer customer);

        Task DeleteCustomer(int id);
    }

    public class ApiCustomerService : IApiCustomerService {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ApiCustomerService(string apiBaseUrl) {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            _apiBaseUrl = apiBaseUrl;
        }

        // GET: Get all customers
        public async Task<List<Customer>> GetAllCustomers() {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Customer>>(json);
            } catch (Exception ex) {
                throw new Exception($"Failed to fetch all customers", ex);
            }
        }

        // GET: Get a single customer by ID
        public async Task<Customer> GetCustomer(int id) {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Customer>(json);
            } catch (Exception ex) {
                throw new Exception($"Failed to fetch customer with ID {id}:", ex);
            }
        }

        // POST: Add a new customer
        public async Task AddCustomer(Customer customer) {
            try {
                var jsonContent = JsonConvert.SerializeObject(customer);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}", content);
                response.EnsureSuccessStatusCode();
            } catch (Exception ex) {
                throw new Exception($"Failed to add customer", ex);
            }
        }

        // PUT: Update a customer
        public async Task UpdateCustomer(Customer customer) {
            try {
                var jsonContent = JsonConvert.SerializeObject(customer);
                System.Diagnostics.Debug.WriteLine($"ApiService.UpdateCustomer=>{jsonContent}");
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                System.Diagnostics.Debug.WriteLine($"ApiService.UpdateCustomer=>StringContent created");
                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{customer.ID}", content);
                System.Diagnostics.Debug.WriteLine($"ApiService.UpdateCustomer=>PUT method executed");
                response.EnsureSuccessStatusCode();
            } catch (Exception ex) {
                throw new Exception($"Failed to update customer with ID {customer.ID}", ex);
            }
        }

        // DELETE: Delete a customer
        public async Task DeleteCustomer(int id) {
            try {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
            } catch (Exception ex) {
                throw new Exception($"Failed to delete customer with ID {id}", ex);
            }
        }
    }
}