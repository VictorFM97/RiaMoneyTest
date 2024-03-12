using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence;
public class CustomerContext : DbContext
{
    public virtual DbSet<Customer> Customers { get; set; }

    public CustomerContext()
        : base()
    {

    }

    public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
