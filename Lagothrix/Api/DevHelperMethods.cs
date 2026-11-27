using Microsoft.AspNetCore.Identity;

namespace Api;

internal static class DevHelperMethods
{
    public static async Task CreateRoles(this WebApplication app, IEnumerable<IdentityRole<Guid>> roles)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in roles)
        {
            string roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}
