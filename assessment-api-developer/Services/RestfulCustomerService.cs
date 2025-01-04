using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using assessment_platform_developer.Models;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using WebGrease.Extensions;

namespace assessment_platform_developer.Services {

    public interface IRestfulCustomerService {

        Task<List<Customer>> GetAllCustomers();

        Task<Customer> GetCustomer(int id);

        Task AddCustomer(Customer customer);

        Task UpdateCustomer(Customer customer);

        Task DeleteCustomer(int id);
    }

    public class RestfulCustomerService : IRestfulCustomerService {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public RestfulCustomerService(HttpClient httpClient) {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            _apiBaseUrl = GetApiBaseUrl();
        }

        private static string GetApiBaseUrl() {
            var apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "https://localhost:44358/api/customers";
            return $"{apiPath}";
        }

        // GET: Get all customers
        public async Task<List<Customer>> GetAllCustomers() {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}");
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Customer>>(json);
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP request failed for GET {_apiBaseUrl}: {ex.Message}", ex);
            } catch (JsonException ex) {
                throw new Exception($"Failed to deserialize response from GET {_apiBaseUrl}: {ex.Message}", ex);
            } catch (Exception ex) {
                throw new Exception($"Failed to fetch all customers", ex);
            }
        }

        // GET: Get a single customer by ID
        public async Task<Customer> GetCustomer(int id) {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Customer>(json);
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP request failed for GET {_apiBaseUrl} by ID: {ex.Message}", ex);
            } catch (JsonException ex) {
                throw new Exception($"Failed to deserialize response from GET {_apiBaseUrl} by ID: {ex.Message}", ex);
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
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP request failed for POST {_apiBaseUrl}: {ex.Message}", ex);
            } catch (JsonException ex) {
                throw new Exception($"Failed to serialize request for POST {_apiBaseUrl}: {ex.Message}", ex);
            } catch (Exception ex) {
                throw new Exception($"Failed to add customer", ex);
            }
        }

        // PUT: Update a customer
        public async Task UpdateCustomer(Customer customer) {
            try {
                var jsonContent = JsonConvert.SerializeObject(customer);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{customer.ID}", content);
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP request failed for PUT {_apiBaseUrl}: {ex.Message}", ex);
            } catch (JsonException ex) {
                throw new Exception($"Failed to serialize request for PUT {_apiBaseUrl}: {ex.Message}", ex);
            } catch (Exception ex) {
                throw new Exception($"Failed to update customer with ID {customer.ID}", ex);
            }
        }

        // DELETE: Delete a customer
        public async Task DeleteCustomer(int id) {
            try {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP request failed for DELETE {_apiBaseUrl} by ID: {ex.Message}", ex);
            } catch (Exception ex) {
                throw new Exception($"Failed to delete customer with ID {id}", ex);
            }
        }
    }
}