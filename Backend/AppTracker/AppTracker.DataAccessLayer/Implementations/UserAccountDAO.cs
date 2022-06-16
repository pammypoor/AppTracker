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
    public class RegistrationDAO: IRegistrationDAO
    {
        private BuildSettingsOptions _options { get; }
        public RegistrationDAO(IOptionsSnapshot<BuildSettingsOptions> options)
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
                    var procedure = "[CreateAccount]";
                    var parameters = new
                    {
                        @email = account.Email,
                        @passphrase = account.Password,
                        @authorization_level = account.AuthorizationLevel,
                        @enabled = account.Enabled,
                        @confirmed = account.Confirmed,
                        @user_hash = userHash
                    };

                    var result = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedure, parameters,
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);



                    return new Response<string>("success", "", 200, true);
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
