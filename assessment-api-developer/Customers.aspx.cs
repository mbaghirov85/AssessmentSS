using assessment_platform_developer.Models;
using assessment_platform_developer.Services;
using assessment_platform_developer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace assessment_platform_developer {

    public partial class Customers : Page {
        private readonly IEmailValidator _emailValidator = new EmailValidator();
        private readonly IPostalCodeValidator _postalCodeValidator = new PostalCodeValidator();

        private IRestfulCustomerService _restfulCustomerService {
            get {
                if (Session["RestfulCustomerService"] == null) {
                    var container = HttpContext.Current.Application["DIContainer"] as SimpleInjector.Container;
                    Session["RestfulCustomerService"] = container.GetInstance<IRestfulCustomerService>();
                }
                return (IRestfulCustomerService)Session["RestfulCustomerService"];
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
                    var customers = await _restfulCustomerService.GetAllCustomers();
                    PopulateDdlCustomers(customers);
                } catch (Exception ex) {
                    ShowMessage("error", $"Error while loading page: {ex}");
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
                ddlCustomers.Items.Add(new ListItem(customer.Name, customer.ID.ToString()));
            }

            ddlCustomers.SelectedValue = "0";
            btnDelete.Visible = false;
            btnAdd.Text = "Add";
            ClearFormFields();
        }

        protected async void btnAdd_Click(object sender, EventArgs e) {
            if (!_emailValidator.IsValidEmail(txtCustomerEmail.Text)) {
                ShowMessage("error", "Invalid customer email format.");
                return;
            }

            if (!String.IsNullOrEmpty(txtContactEmail.Text) && !_emailValidator.IsValidEmail(txtContactEmail.Text)) {
                ShowMessage("error", "Invalid contact email format.");
                return;
            }

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
                ContactPhone = txtContactPhone.Text,
                ContactEmail = txtContactEmail.Text,
                ContactTitle = txtContactTitle.Text,
                ContactNotes = txtContactNotes.Text
            };

            String action = "add";
            try {
                if ((int)Session["customerID"] != 0) {
                    action = "updat";
                    customer.ID = (int)Session["customerID"];
                    await _restfulCustomerService.UpdateCustomer(customer);
                } else {
                    await _restfulCustomerService.AddCustomer(customer);
                }

                // Refresh dropdown and clear form fields
                PopulateDdlCustomers(await _restfulCustomerService.GetAllCustomers());
                ClearFormFields();

                ShowMessage("info", $"Customer {customer.Name} {action}ed successfully! ");
            } catch (Exception ex) {
                ShowMessage("error", $"Error {action}ing customer: {ex.Message}");
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e) {
            try {
                if ((int)Session["customerID"] != 0) {
                    await _restfulCustomerService.DeleteCustomer((int)Session["customerID"]);
                } else {
                    ShowMessage("error", "Please select a proper customer");
                }

                // Refresh dropdown and clear form fields
                PopulateDdlCustomers(await _restfulCustomerService.GetAllCustomers());
                Session["customerID"] = 0;
                ClearFormFields();
                ShowMessage("info", "Customer deleted successfully!");
            } catch (Exception ex) {
                ShowMessage("error", $"Error deleting customer ${(int)Session["customerID"]}: {ex.Message}");
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
                        var customer = await _restfulCustomerService.GetCustomer((int)Session["customerID"]);

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
                        ShowMessage("error", $"Error fetching customer details: {ex.Message}");
                    }
                } else {
                    ShowMessage("error", "Please select a valid customer");
                }
            } else {
                // Clear form fields if "Add new customer" is selected
                ClearFormFields();
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                PopulateDdlState("0");
                revCustomerZip.ValidationExpression = _postalCodeValidator.GetValidationExpression(ddlCountry.SelectedItem.Text);
                txtCustomerZip.Text = "";
                txtCustomerCity.Text = "";
                lblError.Text = "";
            } catch (Exception ex) {
                ShowMessage("error", $"Error populating states/provinces: {ex.Message}");
            }
        }

        public void ShowMessage(string action, string message) {
            if (action == "error") {
                lblError.ForeColor = System.Drawing.Color.Red;
            } else {
                lblError.ForeColor = System.Drawing.Color.Green;
            }
            lblError.Text = message;
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