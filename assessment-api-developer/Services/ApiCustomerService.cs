using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using AssessmentPlatformDeveloper.Models;
using System.Web;
using System.Threading.Tasks;

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
            _apiBaseUrl = apiBaseUrl;
        }

        // GET: Get all customers
        public async Task<List<Customer>> GetAllCustomers() {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}");
                if (response.IsSuccessStatusCode) {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Customer>>(json);
                }
                throw new Exception($"Failed to fetch customers: {response.ReasonPhrase}");
            } catch (HttpRequestException ex) {
                throw new Exception("A network error occurred while fetching customers.", ex);
            }
        }

        // GET: Get a single customer by ID
        public async Task<Customer> GetCustomer(int id) {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                if (response.IsSuccessStatusCode) {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Customer>(json);
                }
                throw new Exception($"Failed to fetch customer with ID {id}: {response.ReasonPhrase}");
            } catch (HttpRequestException ex) {
                throw new Exception("A network error occurred while fetching customers.", ex);
            }
        }

        // POST: Add a new customer
        public async Task AddCustomer(Customer customer) {
            var jsonContent = JsonConvert.SerializeObject(customer);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}", content);
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"Failed to add customer: {response.ReasonPhrase}");
            }
        }

        // PUT: Update a customer
        public async Task UpdateCustomer(Customer customer) {
            var jsonContent = JsonConvert.SerializeObject(customer);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{customer.ID}", content);
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"Failed to update customer with ID {customer.ID}: {response.ReasonPhrase}");
            }
        }

        // DELETE: Delete a customer
        public async Task DeleteCustomer(int id) {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"Failed to delete customer with ID {id}: {response.ReasonPhrase}");
            }
        }
    }
}