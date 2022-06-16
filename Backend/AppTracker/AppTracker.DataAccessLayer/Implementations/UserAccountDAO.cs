using AppTracker.DataAccessLayer.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations.Responses;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class UserAccountDAO: IUserAccountDAO
    {
        private BuildSettingsOptions _options { get; }
        public UserAccountDAO(IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IResponse<string>> CreateUserAccountAsync(IUserAccount account, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using(var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    connection.Open();
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

                    var result = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedure, parameters,
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);



                    return new Response<string>("TEST", _options.SqlConnectionString, 200, true);
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    case -1:
                        return new Response<string>("cannot connect to database", "", 503, false);
                    default:
                        return new Response<string>("unhandled exception" + ex.Message, "", 500, false);
                }
                
            }
            catch(OperationCanceledException ex)
            {
                return new Response<string>("cancellation requested", "", 500, false);
            }
            catch(Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "", 500, false);
            }
        }
    }
}
