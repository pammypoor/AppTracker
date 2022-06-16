using AppTracker.Models;
using AppTracker.Models.Contracts;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class UserAccountDAO
    {
        private BuildSettingsOptions _options { get; }
        public UserAccountDAO(IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IResponse<string>> CreateUserAccount(IUserAccount account, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using(var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[createaccount]";
                    var parameters = new
                    {
                        procedureemail = account.Email,
                        procedurepassphrase = account.Password,
                        procedureauthorizationlevel = account.AuthorizationLevel,
                        procedureenabled = account.Enabled,
                        procedureconfirmed = account.Confirmed,
                        procedurehash = userHash
                    };

                    var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                }
            }
            catch(SqlException ex)
            {

            }
            catch(OperationCanceledException ex)
            {

            }
            catch(Exception ex)
            {

            }
        }

    }
}
