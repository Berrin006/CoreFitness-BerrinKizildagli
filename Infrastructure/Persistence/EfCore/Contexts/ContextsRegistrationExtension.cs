using Domain.Exceptions.Custom;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Infrastructure.Persistence.EfCore.Contexts;

public static class ContextsRegistrationExtension
{
    public static IServiceCollection AddEfCoreContexts(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        if (env.IsDevelopment())
        {
            services.AddDbContext<DataContext>(options =>
            {
                var conn = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("DefaultConnection not found.");

                options.UseSqlServer(conn);
            });
        }
        else
        {
            services.AddDbContext<DataContext>((sp, options) =>
            {
                try
                {
                    var conn = configuration.GetConnectionString("ProductionDatabase")
                        ?? throw new NotFoundDomainException("Production Database ConnectionString Not Found.");

                    options.UseSqlServer(conn);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Console.WriteLine(ex);

                    throw;
                }
            });
        }

        return services;
    }
}
