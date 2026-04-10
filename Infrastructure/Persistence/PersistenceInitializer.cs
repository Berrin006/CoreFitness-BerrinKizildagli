using Infrastructure.Identity;
using Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence;

public static class PersistenceInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IHostEnvironment env, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(env);

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        if (env.IsDevelopment())
        {
            await context.Database.EnsureCreatedAsync(ct);
        }
        else
        {
            await context.Database.MigrateAsync(ct);
        }

        if(!await roleManager.RoleExistsAsync("admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }

        var adminEmail = "admin@domain.com";
        var user = await userManager.FindByEmailAsync(adminEmail);

        if (user != null)
        {
            if(!await userManager.IsInRoleAsync(user, "admin"))
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}