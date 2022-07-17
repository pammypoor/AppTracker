using AppTracker.DataAccessLayer.Contracts;
using AppTracker.DataAccessLayer.Implementations;
using AppTracker.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace AppTracker.Tests.IntegrationTests.DataAccessLayer
{
    public class ProfileDAOShould: TestBaseClass, IClassFixture<ProfileDaoDatabaseFixture>
    {
        ProfileDaoDatabaseFixture databaseFixture;
        public ProfileDAOShould(ProfileDaoDatabaseFixture fixture): base(new string[] { })
        {
            databaseFixture = fixture;
            TestServices.AddScoped<IProfileDAO, ProfileDAO>();
            TestProvider = TestServices.BuildServiceProvider();
        }
    }


    public class ProfileDaoDatabaseFixture: TestBaseClass, IDisposable
    {
        public ProfileDaoDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/DataAccessLayer/SetupAndCleanup/AuthorizationDAOSetup.sql");
            IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using(var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                foreach(string command in commands)
                {
                    if (!string.IsNullOrWhiteSpace(command.Trim()))
                    {
                        using (var com = new SqlCommand(command, connection))
                        {
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
