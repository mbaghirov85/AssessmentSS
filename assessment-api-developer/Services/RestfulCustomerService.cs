using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using assessment_platform_developer.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using assessment_platform_developer.Helpers;

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
        private readonly HttpBodyHelper _httpBodyHelper;

        public RestfulCustomerService(HttpClient httpClient) {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            _apiBaseUrl = GetApiBaseUrl();

            _httpBodyHelper = new HttpBodyHelper();
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
                throw new Exception($"HTTP GET request failed: {ex.Message}");
            } catch (JsonException ex) {
                throw new Exception($"Failed to deserialize GET response: {ex.Message}");
            } catch (Exception ex) {
                throw new Exception($"Failed to fetch all customers: {ex.Message}");
            }
        }

        // GET: Get a single customer by ID
        public async Task<Customer> GetCustomer(int id) {
            try {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Customer>(json);
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP GET by ID request failed: {ex.Message}");
            } catch (JsonException ex) {
                throw new Exception($"Failed to deserialize GET by ID response: {ex.Message}");
            } catch (Exception ex) {
                throw new Exception($"Failed to fetch customer with ID {id}: {ex.Message}");
            }
        }

        // POST: Add a new customer
        public async Task AddCustomer(Customer customer) {
            string body = "";
            try {
                var jsonContent = JsonConvert.SerializeObject(customer);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}", content);
                body = await _httpBodyHelper.GetBodyMessage(response);
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP POST request failed: {body}");
            } catch (JsonException ex) {
                throw new Exception($"Failed to serialize POST request: [{body}] {ex.Message}");
            } catch (Exception ex) {
                throw new Exception($"Failed to add customer: [{body}] {ex.Message}");
            }
        }

        // PUT: Update a customer
        public async Task UpdateCustomer(Customer customer) {
            string body = "";
            try {
                var jsonContent = JsonConvert.SerializeObject(customer);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{customer.ID}", content);

                body = await _httpBodyHelper.GetBodyMessage(response);

                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP PUT request failed: {body}");
            } catch (JsonException ex) {
                throw new Exception($"Failed to serialize PUT request: [{body}] {ex.Message}");
            } catch (Exception ex) {
                throw new Exception($"Failed to update customer with ID {customer.ID}: [{body}] {ex.Message}");
            }
        }

        // DELETE: Delete a customer
        public async Task DeleteCustomer(int id) {
            string body = "";
            try {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
                body = await _httpBodyHelper.GetBodyMessage(response);
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                throw new Exception($"HTTP DELETE by ID request failed: {body}");
            } catch (Exception ex) {
                throw new Exception($"Failed to delete customer with ID {id}: [{body}] {ex.Message}");
            }
        }
    }
}