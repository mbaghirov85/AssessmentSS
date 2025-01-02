using AssessmentPlatformDeveloper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssessmentPlatformDeveloper.Services;
using Container = SimpleInjector.Container;

namespace AssessmentPlatformDeveloper {

    public partial class Customers : Page {
        private static List<Customer> customers = new List<Customer>();
        private static List<ListItem> customersList;
        private static List<ListItem> countries;
        private static List<ListItem> provinces;
        private static List<ListItem> states;

        protected void Page_Load(object sender, EventArgs e) {
            if (Request.HttpMethod == "GET" && Request.Url.AbsolutePath.Contains("/api/customers")) {
                var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
                var customerService = testContainer.GetInstance<ICustomerService>();

                var allCustomers = customerService.GetAllCustomers();

                // Serialize customers to JSON and write to response
                Response.ContentType = "application/json";
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(allCustomers));
                Response.End();
            }

            if (!IsPostBack) {
                var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
                var customerService = testContainer.GetInstance<ICustomerService>();

                var allCustomers = customerService.GetAllCustomers();
                customers = ViewState["Customers"] as List<Customer> ?? new List<Customer>();

                InitLists();
            } else {
                customers = (List<Customer>)ViewState["Customers"];
            }

            //not a proper method. to be fixed
            PopulateDdlList(ddlCustomers, customers);
            PopulateDdlList(ddlCountry, countries);
            PopulateDdlList(ddlState, provinces);
        }

        private void InitLists() {
            countries = Enum.GetValues(typeof(Countries))
                .Cast<object>()
                .OfType<Countries>()
                .Select(item => new ListItem {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                })
                .ToList();

            provinces = Enum.GetValues(typeof(CanadianProvinces))
                .Cast<object>()
                .OfType<CanadianProvinces>()
                .Select(item => new ListItem {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                })
                .ToList();

            states = Enum.GetValues(typeof(USStates))
                .Cast<object>()
                .OfType<USStates>()
                .Select(item => new ListItem {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                })
                .ToList();
        }

        private void PopulateDdlList(DropDownList ddl, IEnumerable<ListItem> items) {
            ddl.Items.Clear();
            if (ddl.ID == ddlState.ID) {
                ddl.Items.Add(new ListItem(""));
            }
            ddl.Items.AddRange(items.ToArray());
        }

        protected void PopulateDdlCustomers() {
            ddlCustomers.Items.Clear();
            // moving addition of the new customer to the top
            ddlCustomers.Items.Add(new ListItem("Add new customer"));

            var storedCustomers = customers.Select(c => new ListItem(c.Name)).ToArray();
            if (storedCustomers.Length != 0) {
                ddlCustomers.Items.AddRange(storedCustomers);
                ddlCustomers.SelectedIndex = 0;
                return;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e) {
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

            var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
            var customerService = testContainer.GetInstance<ICustomerService>();
            customerService.AddCustomer(customer);
            customers.Add(customer);

            // replacing list item addition with the designated method because there's a change that someone else will add a new customer.
            //ddlCustomers.Items.Add(new ListItem(customer.Name));
            PopulateDdlCustomers();

            txtCustomerName.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            txtCustomerEmail.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            txtCustomerCity.Text = string.Empty;
            ddlCountry.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            txtCustomerZip.Text = string.Empty;
            txtCustomerNotes.Text = string.Empty;
            txtContactName.Text = string.Empty;
            txtContactTitle.Text = string.Empty;
            txtContactNotes.Text = string.Empty;
            txtContactPhone.Text = string.Empty;
            txtContactEmail.Text = string.Empty;
        }
    }
}