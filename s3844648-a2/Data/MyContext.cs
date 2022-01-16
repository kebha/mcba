using Microsoft.EntityFrameworkCore;
using s3844648_a2.Models;

namespace s3844648_a2.Data;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    public DbSet<Customer> Customer { get; set; }
}

