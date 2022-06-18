using AppTracker.Models;
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
        }

    }
}
