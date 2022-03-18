using App.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace App;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        var repository = host.Services.GetRequiredService<IEmployeeRepository>();
        var getResults = await repository.GetEmployeesAsync();
        var findResults = await repository.FindEmployeesAsync();
        DumpEmployees("Get Employees Results", getResults);
        DumpEmployees("Find Employees Results", findResults);
        ConsoleColor.Yellow.WriteLine("Press any key to exit !");
        Console.ReadKey();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddUserSecrets();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services.AddTransient<IEmployeeRepository, EmployeeRepository>();
                services.AddTransient<IOracleProvider>(_ =>
                {
                    var connectionString = hostingContext.Configuration.GetConnectionString("OracleDbConnection");
                    return new OracleProvider(new OracleConnection(connectionString));
                });
            })
            .UseConsoleLifetime();

    private static void DumpEmployees(string description, ICollection<Employee> employees)
    {
        ConsoleColor.Green.WriteLine(description);
        Console.WriteLine($"Found {employees.Count} employees");
        Console.WriteLine(ObjectDumper.Dump(employees));
        Console.WriteLine();
    }
}