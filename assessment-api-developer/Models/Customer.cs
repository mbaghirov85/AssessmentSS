using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace assessment_platform_developer.Models
{
	public enum Countries
	{
		Canada,
		[Description("United States")]
		UnitedStates
	}
	public enum USStates
	{
		Alabama,
		Alaska,
		Arizona,
		Arkansas,
		California,
		Colorado,
		Connecticut,
		Delaware,
		Florida,
		Georgia,
		Hawaii,
		Idaho,
		Illinois,
		Indiana,
		Iowa,
		Kansas,
		Kentucky,
		Louisiana,
		Maine,
		Maryland,
		Massachusetts,
		Michigan,
		Minnesota,
		Mississippi,
		Missouri,
		Montana,
		Nebraska,
		Nevada,
		[Description("New Hampshire")]
		NewHampshire,
		[Description("New Jersey")]
		NewJersey,
		[Description("New Mexico")]
		NewMexico,
		[Description("New York")]
		NewYork,
		[Description("North Carolina")]
		NorthCarolina,
		[Description("North Dakota")]
		NorthDakota,
		Ohio,
		Oklahoma,
		Oregon,
		Pennsylvania,
		[Description("Rhode Island")]
		RhodeIsland,
		[Description("South Carolina")]
		SouthCarolina,
		[Description("South Dakota")]
		SouthDakota,
		Tennessee,
		Texas,
		Utah,
		Vermont,
		Virginia,
		Washington,
		[Description("West Virginia")]
		WestVirginia,
		Wisconsin,
		Wyoming
	}

	public enum CanadianProvinces
	{
		Alberta,
		[Description("British Columbia")]
		BritishColumbia,
		Manitoba,
		NewBrunswick,
		[Description("Newfoundland and Labrador")]
		NewfoundlandAndLabrador,
		[Description("Northwest Territories")]
		NovaScotia,
		Ontario,
		[Description("Prince Edward Island")]
		PrinceEdwardIsland,
		Quebec,
		Saskatchewan,
		[Description("Yukon")]
		NorthwestTerritories,
		Nunavut,
		Yukon
	}

	[Serializable]
	public class Customer
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public string Notes { get; set; }
		public string ContactName { get; set; }
		public string ContactPhone { get; set; }
		public string ContactEmail { get; set; }
		public string ContactTitle { get; set; }
		public string ContactNotes { get; set; }

	}

	public class CustomerDBContext : DbContext
	{
		public DbSet<Customer> Customers { get; set; }
	}
}