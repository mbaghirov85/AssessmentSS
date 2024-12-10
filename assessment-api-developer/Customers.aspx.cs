using assessment_platform_developer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using assessment_platform_developer.Services;
using Container = SimpleInjector.Container;

namespace assessment_platform_developer
{
	public partial class Customers : Page
	{
		private static List<Customer> customers = new List<Customer>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
				var customerService = testContainer.GetInstance<ICustomerService>();

				var allCustomers = customerService.GetAllCustomers();
				ViewState["Customers"] = allCustomers;
			}
			else
			{
				customers = (List<Customer>)ViewState["Customers"];
			}

			PopulateCustomerListBox();
			PopulateCustomerDropDownLists();
		}

		private void PopulateCustomerDropDownLists()
		{

			var countryList = Enum.GetValues(typeof(Countries))
				.Cast<Countries>()
				.Select(c => new ListItem
				{
					Text = c.ToString(),
					Value = ((int)c).ToString()
				})
				.ToArray();

			CountryDropDownList.Items.AddRange(countryList);
			CountryDropDownList.SelectedValue = ((int)Countries.Canada).ToString();


			var provinceList = Enum.GetValues(typeof(CanadianProvinces))
				.Cast<CanadianProvinces>()
				.Select(p => new ListItem
				{
					Text = p.ToString(),
					Value = ((int)p).ToString()
				})
				.ToArray();

			StateDropDownList.Items.Add(new ListItem(""));
			StateDropDownList.Items.AddRange(provinceList);
		}

		protected void PopulateCustomerListBox()
		{
			CustomersDDL.Items.Clear();
			var storedCustomers = customers.Select(c => new ListItem(c.Name)).ToArray();
			if (storedCustomers.Length != 0)
			{
				CustomersDDL.Items.AddRange(storedCustomers);
				CustomersDDL.SelectedIndex = 0;
				return;
			}

			CustomersDDL.Items.Add(new ListItem("Add new customer"));
		}

		protected void AddButton_Click(object sender, EventArgs e)
		{
			var customer = new Customer
			{
				Name = CustomerName.Text,
				Address = CustomerAddress.Text,
				City = CustomerCity.Text,
				State = StateDropDownList.SelectedValue,
				Zip = CustomerZip.Text,
				Country = CountryDropDownList.SelectedValue,
				Email = CustomerEmail.Text,
				Phone = CustomerPhone.Text,
				Notes = CustomerNotes.Text,
				ContactName = ContactName.Text,
				ContactPhone = CustomerPhone.Text,
				ContactEmail = CustomerEmail.Text
			};

			var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
			var customerService = testContainer.GetInstance<ICustomerService>();
			customerService.AddCustomer(customer);
			customers.Add(customer);

			CustomersDDL.Items.Add(new ListItem(customer.Name));

			CustomerName.Text = string.Empty;
			CustomerAddress.Text = string.Empty;
			CustomerEmail.Text = string.Empty;
			CustomerPhone.Text = string.Empty;
			CustomerCity.Text = string.Empty;
			StateDropDownList.SelectedIndex = 0;
			CustomerZip.Text = string.Empty;
			CountryDropDownList.SelectedIndex = 0;
			CustomerNotes.Text = string.Empty;
			ContactName.Text = string.Empty;
			ContactPhone.Text = string.Empty;
			ContactEmail.Text = string.Empty;
		}
	}
}