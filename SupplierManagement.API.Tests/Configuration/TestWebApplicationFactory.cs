using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace SupplierManagement.API.Tests.Configuration
{
    public class TestAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public static class TestWebApplicationFactory
    {
        public static WebApplicationFactory<Program> CreateWithMockedAuth()
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove existing authorization handlers
                        services.AddSingleton<IAuthorizationHandler, TestAuthorizationHandler>();
                    });
                });
        }
    }
}
