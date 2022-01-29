using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using s3844648_a2.Data;
using s3844648_a2.Models;

namespace s3844648_a2.BackgroundServices;

// This class was built off code from the Day 8 Lectorial code
public class BillPayService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<BillPayService> _logger;

    public BillPayService(IServiceProvider services, ILogger<BillPayService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await ProcessBill(cancellationToken);

            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }

    private async Task ProcessBill(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyContext>();

        var billPays = await context.BillPays.Where(x => x.ScheduleTimeUtc <= DateTime.UtcNow).ToListAsync(cancellationToken);
        foreach (var billPay in billPays)
        {
            // Check if account has enough funds
            if (billPay.Account.AccountType == AccountType.Savings && billPay.Amount <= billPay.Account.Balance || billPay.Account.AccountType == AccountType.Checking && billPay.Amount <= billPay.Account.Balance - 300)
            {
                // Update Balance & add transaction
                var account = await context.Accounts.FindAsync(billPay.AccountID);
                account.Balance -= billPay.Amount;
                await context.Transactions.AddAsync(new Transaction
                {
                    TransactionType = TransactionType.BillPay,
                    AccountID = billPay.AccountID,
                    DestinationAccountID = null,
                    Amount = billPay.Amount,
                    Comment = null,
                    TransactionTimeUtc = DateTime.UtcNow
                });

                // Increment BillPay if Monthly
                if (billPay.Period == Period.Monthly)
                    billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.AddMonths(1);
                else
                    context.BillPays.Remove(billPay);

            }
            else
                context.BillPays.Remove(billPay);
        }

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("BillPay processed!");
    }
}
