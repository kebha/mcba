using Microsoft.AspNetCore.Mvc;

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
        _logger.LogInformation("People Background Service is running.");

        while (!cancellationToken.IsCancellationRequested)
        {
            await DoWork(cancellationToken);

            _logger.LogInformation("People Background Service is waiting a minute.");

            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        _logger.LogInformation("People Background Service is working.");

        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PeopleContext>();

        var persons = await context.Persons.ToListAsync(cancellationToken);
        foreach (var person in persons)
        {
            // Name's starting with an S have count incremented by 5, everyone else has count incremented by 1.
            if (person.Name.StartsWith('S'))
                person.Count += 5;
            else
                person.Count++;
        }

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("People Background Service work complete.");
    }
}
