using s3844648_a2.Models;
using Newtonsoft.Json;

namespace s3844648_a2.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<MyContext>();

        // Look for customers.
        if (context.Customers.Any())
            return;

        const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

        // Contact webservice.
        using var client = new HttpClient();
        var json = client.GetStringAsync(Url).Result;

        // Convert JSON into objects.
        var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
        {
            DateFormatString = "dd/MM/yyyy"
        });

        

        // Insert into database.
        foreach (var customer in customers)
        {
            // Insert Customer
            context.Customers.Add(new Customer
            {
                CustomerID = customer.CustomerID,
                Name = customer.Name,
                Address = customer.Address,
                Suburb = customer.Suburb,
                PostCode = customer.PostCode
            });

            // Insert Login
            context.Logins.Add(new Login
            {
                LoginID = customer.Login.LoginID,
                CustomerID = customer.CustomerID,
                PasswordHash = customer.Login.PasswordHash
            });

            foreach (var account in customer.Accounts)
            {
                // Insert Account
                account.Balance = 0;
                foreach (var transaction in account.Transactions)
                {
                    account.Balance += transaction.Amount;
                }
                context.Accounts.Add(new Account
                {
                    AccountID = account.AccountID,
                    AccountType = account.AccountType,
                    CustomerID = account.CustomerID,
                    Balance = account.Balance
                });

                foreach (var transaction in account.Transactions)
                {
                    // Insert Transactions
                    context.Transactions.Add(new Transaction
                    {
                        AccountID = account.AccountID,
                        TransactionType = TransactionType.Deposit,
                        Amount = transaction.Amount,
                        Comment = transaction.Comment,
                        TransactionTimeUtc = transaction.TransactionTimeUtc
                    });
                }
            }
        }
        context.SaveChanges();
    }
}
