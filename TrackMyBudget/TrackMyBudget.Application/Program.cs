using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Events;
using Amazon;
using Microsoft.EntityFrameworkCore;
using TrackMyBudget.Infrastructure.Data;
using TrackMyBudget.Core.Contract;
using TrackMyBudget.Infrastructure.Implementation;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Read LogGroupName from configuration
        var logGroupName = builder.Configuration["Serilog:CloudWatchLogGroupName"];

        // Clear default logging providers to avoid duplicate logging
        builder.Logging.ClearProviders();  // Removes default logging providers like Console, Debug, etc.

        // Correct Serilog setup to read from appsettings.json
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)  // Reads Serilog config from appsettings.json
                .Enrich.FromLogContext()  // Adds contextual information to logs
                .WriteTo.Console()  // Optional: Logs to console
                .WriteTo.AmazonCloudWatch(new CloudWatchSinkOptions
                {
                    LogGroupName = logGroupName,  // CloudWatch log group name
                    MinimumLogEventLevel = LogEventLevel.Information,  // Minimum log level
                    CreateLogGroup = true,  // Create the log group if not exists
                    LogStreamNameProvider = new DefaultLogStreamProvider(),
                    TextFormatter = new Serilog.Formatting.Compact.CompactJsonFormatter()  // JSON format for structured logs
                }, new AmazonCloudWatchLogsClient(RegionEndpoint.APSoutheast1));  // AWS CloudWatch Logs client
        });

        // Register ApplicationDbContext with the DI container
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register DB Migration service 
        builder.Services.AddScoped<DatabaseMigrationService>();

        // Register UnitOfWork and repositories with DI
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

        // Register Health Check services
        builder.Services.AddHealthChecks();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Call migration service at startup
        using (var scope = app.Services.CreateScope())
        {
            var migrationService = scope.ServiceProvider.GetRequiredService<DatabaseMigrationService>();
            migrationService.MigrateDatabase();  // Call the migration logic
        }

        // Enable health check endpoint
        app.UseHealthChecks("/health");

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();

        Log.CloseAndFlush();
    }
}