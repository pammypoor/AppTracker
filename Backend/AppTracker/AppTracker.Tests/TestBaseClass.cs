using AppTracker.DataAccessLayer.Contracts;
using AppTracker.DataAccessLayer.Implementations;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Services.Contracts;
using AppTracker.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppTracker.Tests
{
    public class TestBaseClass
    {
        public IServiceCollection TestServices;
        public IServiceProvider TestProvider;

        public TestBaseClass(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            TestServices = new ServiceCollection();
            TestServices.Configure<BuildSettingsOptions>(config.GetSection(nameof(BuildSettingsOptions)));
            TestServices.AddScoped<IMessageBank, AppTracker.MessageBank.Implementations.MessageBank>();
            TestServices.AddScoped<IAuthorizationDAO, AuthorizationDAO>();
            TestServices.AddScoped<IAuthorizationService, AuthorizationService>();
        }
    }
}
