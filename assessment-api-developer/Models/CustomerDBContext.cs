using System.Data.Entity;

namespace AssessmentPlatformDeveloper.Models {

    public class CustomerDBContext : DbContext {
        public DbSet<Customer> Customers { get; set; }
    }
}