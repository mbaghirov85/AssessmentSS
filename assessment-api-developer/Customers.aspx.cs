using AssessmentPlatformDeveloper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssessmentPlatformDeveloper.Services;
using Container = SimpleInjector.Container;
using System.Web;

namespace AssessmentPlatformDeveloper {

    public partial class Customers : Page {
        private IApiCustomerService _apiCustomerService;

        private List<ListItem> countries {
            get => ViewState["Countries"] as List<ListItem> ?? new List<ListItem>();
            set => ViewState["Countries"] = value;
        }

        private List<ListItem> provinces {
            get => ViewState["Provinces"] as List<ListItem> ?? new List<ListItem>();
            set => ViewState["Provinces"] = value;
        }

        private List<ListItem> states {
            get => ViewState["States"] as List<ListItem> ?? new List<ListItem>();
            set => ViewState["States"] = value;
        }

        private static int customerID = 0;

        protected async void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                string apiBaseUrl = GetApiBaseUrl();
                var container = HttpContext.Current.Application["DIContainer"] as SimpleInjector.Container;
                _apiCustomerService = container.GetInstance<IApiCustomerService>();

                try {
                    var customers = await _apiCustomerService.GetAllCustomers();
                    PopulateDdlCustomers(customers);
                } catch (Exception ex) {
                    lblError.Text = $"Error while loading page: {ex.ToString()}";
                }

                InitLists();
                PopulateDdlCountry();
            }
        }

        private string GetApiBaseUrl() {
            var scheme = HttpContext.Current.Request.Url.Scheme; // "http" or "https"
            var authority = HttpContext.Current.Request.Url.Authority; // "localhost:1234" or "www.example.com"
            var appPath = HttpContext.Current.Request.ApplicationPath.TrimEnd('/'); // add virtual directory

            // Retrieve API path from configuration
            var apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiPath"] ?? "/api/customers";

            return $"{scheme}://{authority}{appPath}{apiPath}";
        }

        private void InitLists() {
            countries = Enum.GetValues(typeof(Countries))
                .Cast<Countries>()
                .Select(item => new ListItem {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                })
                .ToList();

            provinces = Enum.GetValues(typeof(CanadianProvinces))
                .Cast<CanadianProvinces>()
                .Select(item => new ListItem {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                })
                .ToList();

            states = Enum.GetValues(typeof(USStates))
                .Cast<USStates>()
                .Select(item => new ListItem {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                })
                .ToList();
        }

        private void PopulateDdlCountry() {
            ddlCountry.Items.Clear();
            ddlCountry.Items.Add(new ListItem("Select country", "0"));
            ddlCountry.Items.AddRange(countries.ToArray());
            ddlCountry.SelectedIndex = 0;
        }

        private void PopulateDdlState(string selectedValue) {
            ddlState.Items.Clear();
            ddlState.Items.Add(new ListItem("", "0"));

            ddlState.SelectedIndex = int.Parse(selectedValue);

            string selectedCountry = ddlCountry.SelectedValue;
            // Populate ddlState based on selected country
            if (selectedCountry == ((int)Countries.Canada).ToString()) { // Canada
                ddlState.Items.AddRange(provinces.ToArray());
                lblCustomerState.Text = "Province";
            } else if (selectedCountry == ((int)Countries.UnitedStates).ToString()) { // United States
                ddlState.Items.AddRange(states.ToArray());
                lblCustomerState.Text = "State";
            }
        }

        private void PopulateDdlCustomers(List<Customer> customers) {
            ddlCustomers.Items.Clear();
            ddlCustomers.Items.Add(new ListItem("Add new customer"));

            foreach (var customer in customers) {
                ddlCustomers.Items.Add(new ListItem(customer.Name, customer.ID.ToString()));
            }

            ddlCustomers.SelectedIndex = 0;
            btnDelete.Visible = false;
            btnAdd.Text = "Add";
        }

        protected async void btnAdd_Click(object sender, EventArgs e) {
            var customer = new Customer {
                Name = txtCustomerName.Text,
                Address = txtCustomerAddress.Text,
                Email = txtCustomerEmail.Text,
                Phone = txtCustomerPhone.Text,
                City = txtCustomerCity.Text,
                State = ddlState.SelectedValue,
                Zip = txtCustomerZip.Text,
                Country = ddlCountry.SelectedValue,
                Notes = txtCustomerNotes.Text,
                ContactName = txtContactName.Text,
                ContactPhone = txtCustomerPhone.Text,
                ContactEmail = txtCustomerEmail.Text,
                ContactTitle = txtContactTitle.Text,
                ContactNotes = txtContactNotes.Text
            };

            try {
                if (customerID != 0) {
                    customer.ID = customerID;
                    await _apiCustomerService.UpdateCustomer(customer);
                } else {
                    await _apiCustomerService.AddCustomer(customer);
                }

                // Refresh dropdown and clear form fields
                PopulateDdlCustomers(await _apiCustomerService.GetAllCustomers());
                ClearFormFields();

                lblError.Text = "Customer added successfully!";
            } catch (Exception ex) {
                lblError.Text = $"Error adding customer: {ex.Message}";
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e) {
            try {
                if (customerID != 0) {
                    _apiCustomerService.DeleteCustomer(customerID);
                } else {
                    lblError.Text = "Please select a proper customer";
                }

                // Refresh dropdown and clear form fields
                PopulateDdlCustomers(await _apiCustomerService.GetAllCustomers());
                ClearFormFields();

                lblError.Text = "Customer deleted successfully!";
            } catch (Exception ex) {
                lblError.Text = $"Error deleting customer ${customerID}: {ex.Message}";
            }
        }

        protected async void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e) {
            // If real cutomer is selected
            if (ddlCustomers.SelectedIndex > 0) {
                customerID = int.Parse(ddlCustomers.SelectedValue);
                btnDelete.Visible = true;
                try {
                    // Get customer details
                    var customer = await _apiCustomerService.GetCustomer(customerID);

                    // Populate form fields with retrieved customer data
                    txtCustomerName.Text = customer.Name;
                    txtCustomerAddress.Text = customer.Address;
                    txtCustomerEmail.Text = customer.Email;
                    txtCustomerPhone.Text = customer.Phone;
                    txtCustomerCity.Text = customer.City;
                    ddlCountry.SelectedValue = customer.Country;
                    PopulateDdlState(customer.State);
                    txtCustomerZip.Text = customer.Zip;
                    txtCustomerNotes.Text = customer.Notes;
                    txtContactName.Text = customer.ContactName;
                    txtContactPhone.Text = customer.ContactPhone;
                    txtContactEmail.Text = customer.ContactEmail;
                    txtContactTitle.Text = customer.ContactTitle;
                    txtContactNotes.Text = customer.ContactNotes;

                    btnAdd.Text = "Update";
                    lblError.Text = "";
                } catch (Exception ex) {
                    lblError.Text = $"Error fetching customer details: {ex.Message}";
                }
            } else {
                // Clear form fields if "Add new customer" is selected
                ClearFormFields();
                btnAdd.Text = "Add";
                btnDelete.Visible = false;
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                PopulateDdlState("0");
                txtCustomerZip.Text = "";
                lblError.Text = "";
            } catch (Exception ex) {
                lblError.Text = $"Error populating states/provinces: {ex.Message}";
            }
        }

        public void ClearFormFields() {
            txtCustomerName.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            txtCustomerEmail.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            txtCustomerCity.Text = string.Empty;
            ddlCountry.SelectedIndex = 0;
            PopulateDdlState("0");
            txtCustomerZip.Text = string.Empty;
            txtCustomerNotes.Text = string.Empty;
            txtContactName.Text = string.Empty;
            txtContactTitle.Text = string.Empty;
            txtContactNotes.Text = string.Empty;
            txtContactPhone.Text = string.Empty;
            txtContactEmail.Text = string.Empty;
            lblError.Text = "";
        }
    }
}