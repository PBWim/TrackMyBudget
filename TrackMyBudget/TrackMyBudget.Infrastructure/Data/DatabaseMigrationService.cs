using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TrackMyBudget.Infrastructure.Data
{
    public class DatabaseMigrationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DatabaseMigrationService> _logger;

        public DatabaseMigrationService(ApplicationDbContext dbContext, ILogger<DatabaseMigrationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void MigrateDatabase()
        {
            try
            {
                _logger.LogInformation("Starting database migration...");

                // Apply pending migrations
                _dbContext.Database.Migrate();

                _logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database.");
                throw; // Optionally re-throw or handle the error
            }
        }
    }
}