using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssessmentPlatformDeveloper.Models
{
    public class CustomerDBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}