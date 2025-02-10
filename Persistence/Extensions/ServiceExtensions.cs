using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistence.Interfaces;
using Persistence.Interfaces.Auditing;
using Persistence.Repositories;
using Persistence.Repositories.Auditing;

namespace Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<ICoverRepository, CoverRepository>();
        services.AddScoped<IClaimAuditRepository, ClaimAuditRepository>();
        services.AddScoped<ICoverAuditRepository, CoverAuditRepository>();

        var auditDbConnString = configuration.GetConnectionString("DefaultConnection");
        var platformDbConnString = configuration.GetConnectionString("MongoDb");
        var mongoDb = configuration["MongoDb:DatabaseName"];

        services.AddDbContextFactory<AuditDbContext>(options => 
            options.UseSqlServer(auditDbConnString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddDbContext<PlatformDbContext>(
             options =>
             {
                 var client = new MongoClient(platformDbConnString);
                 var database = client.GetDatabase(mongoDb);
                 options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
             }
        );
    }

    public static void ApplyAuditDbMigration(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
        context.Database.Migrate();
    }
}
