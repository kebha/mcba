using Microsoft.EntityFrameworkCore;
using s3844648_a2.Models;

namespace s3844648_a2.Data;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Payee> Payees { get; set; }
    public DbSet<BillPay> BillPays { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8").
            HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
        builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0");
        builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");
    }
}

