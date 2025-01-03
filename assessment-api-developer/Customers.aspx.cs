using AssessmentPlatformDeveloper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssessmentPlatformDeveloper.Services;
using AssessmentPlatformDeveloper.Helpers;
using Container = SimpleInjector.Container;
using System.Web;

namespace AssessmentPlatformDeveloper {

    public partial class Customers : Page {

        private IApiCustomerService _apiCustomerService {
            get {
                if (Session["ApiCustomerService"] == null) {
                    var container = HttpContext.Current.Application["DIContainer"] as SimpleInjector.Container;
                    Session["ApiCustomerService"] = container.GetInstance<IApiCustomerService>();
                }
                return (IApiCustomerService)Session["ApiCustomerService"];
            }
        }

        private List<ListItem> countries {
            get {
                return Session["Countries"] as List<ListItem>;
            }
            set {
                Session["Countries"] = value;
            }
        }

        private List<ListItem> provinces {
            get {
                return Session["Provinces"] as List<ListItem>;
            }
            set {
                Session["Provinces"] = value;
            }
        }

        private List<ListItem> states {
            get {
                return Session["States"] as List<ListItem>;
            }
            set {
                Session["States"] = value;
            }
        }

        protected async void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                try {
                    var customers = await _apiCustomerService.GetAllCustomers();
                    PopulateDdlCustomers(customers);
                } catch (Exception ex) {
                    lblError.Text = $"Error while loading page: {ex}";
                }

                InitLists();
                PopulateDdlCountry();
                Session["customerID"] = 0;
            }
        }

        private void InitLists() {
            countries = Enum.GetValues(typeof(Countries))
                .Cast<Countries>()
                .Select(item => new ListItem {
                    Text = EnumExtensions.GetEnumDescription(item),
                    Value = ((int)item).ToString()
                })
                .ToList();
            Session["Countries"] = countries;

            provinces = Enum.GetValues(typeof(CanadianProvinces))
                .Cast<CanadianProvinces>()
                .Select(item => new ListItem {
                    Text = EnumExtensions.GetEnumDescription(item),
                    Value = ((int)item).ToString()
                })
                .ToList();
            Session["Provinces"] = provinces;

            states = Enum.GetValues(typeof(USStates))
                .Cast<USStates>()
                .Select(item => new ListItem {
                    Text = EnumExtensions.GetEnumDescription(item),
                    Value = ((int)item).ToString()
                })
                .ToList();
            Session["States"] = states;
        }

        private void PopulateDdlCountry() {
            ddlCountry.Items.Clear();
            ddlCountry.Items.AddRange(countries.ToArray());
            ddlCountry.SelectedIndex = 0;
        }

        private void PopulateDdlState(string selectedValue) {
            ddlState.Items.Clear();
            string selectedCountry = ddlCountry.SelectedValue;
            // Populate ddlState based on selected country
            if (selectedCountry == ((int)Countries.Canada).ToString()) { // Canada
                ddlState.Items.AddRange(provinces.ToArray());
                lblCustomerState.Text = "Province";
            } else if (selectedCountry == ((int)Countries.UnitedStates).ToString()) { // United States
                ddlState.Items.AddRange(states.ToArray());
                lblCustomerState.Text = "State";
            }
            ddlState.SelectedValue = selectedValue;
        }

        private void PopulateDdlCustomers(List<Customer> customers) {
            ddlCustomers.Items.Clear();
            ddlCustomers.Items.Add(new ListItem("Add new customer", "0"));

            foreach (var customer in customers) {
                System.Diagnostics.Debug.WriteLine($"Customer from Customers.aspx.DdlCustomers {customer.ID} with the name {customer.Name}");
                ddlCustomers.Items.Add(new ListItem(customer.Name, customer.ID.ToString()));
            }

            ddlCustomers.SelectedValue = "0";
            btnDelete.Visible = false;
            btnAdd.Text = "Add";
            ClearFormFields();
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

            String action = "add";
            try {
                System.Diagnostics.Debug.WriteLine($"btnAdd=>{Session["customerID"]}");
                if ((int)Session["customerID"] != 0) {
                    System.Diagnostics.Debug.WriteLine($"btnAdd=>Updating");
                    customer.ID = (int)Session["customerID"];
                    await _apiCustomerService.UpdateCustomer(customer);
                } else {
                    action = "updat";
                    System.Diagnostics.Debug.WriteLine($"btnAdd=>Adding");
                    await _apiCustomerService.AddCustomer(customer);
                }

                // Refresh dropdown and clear form fields
                PopulateDdlCustomers(await _apiCustomerService.GetAllCustomers());
                ClearFormFields();

                lblError.Text = $"Customer {customer.Name} {action}ed successfully! ";
            } catch (Exception ex) {
                lblError.Text = $"Error {action}ing customer: {ex.Message}";
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e) {
            try {
                if ((int)Session["customerID"] != 0) {
                    await _apiCustomerService.DeleteCustomer((int)Session["customerID"]);
                } else {
                    lblError.Text = "Please select a proper customer";
                }

                // Refresh dropdown and clear form fields
                PopulateDdlCustomers(await _apiCustomerService.GetAllCustomers());
                Session["customerID"] = 0;
                ClearFormFields();
                lblError.Text = "Customer deleted successfully!";
            } catch (Exception ex) {
                lblError.Text = $"Error deleting customer ${(int)Session["customerID"]}: {ex.Message}";
            }
        }

        protected async void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e) {
            // If real cutomer is selected
            if (ddlCustomers.SelectedIndex > 0) {
                lblFormCaption.Text = "Update Customer";
                if (!string.IsNullOrEmpty(ddlCustomers.SelectedValue)) {
                    Session["customerID"] = int.Parse(ddlCustomers.SelectedValue);
                    btnDelete.Visible = true;
                    try {
                        // Get customer details
                        var customer = await _apiCustomerService.GetCustomer((int)Session["customerID"]);

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
                    lblError.Text = "Please select a valid customer";
                }
            } else {
                // Clear form fields if "Add new customer" is selected
                ClearFormFields();
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                PopulateDdlState("0");
                txtCustomerZip.Text = "";
                txtCustomerCity.Text = "";
                lblError.Text = "";
            } catch (Exception ex) {
                lblError.Text = $"Error populating states/provinces: {ex.Message}";
            }
        }

        public void ClearFormFields() {
            Session["customerID"] = 0;
            ddlCustomers.SelectedValue = "0";
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
            lblFormCaption.Text = "Add customer";
            btnAdd.Text = "Add";
            btnDelete.Visible = false;
        }
    }
}