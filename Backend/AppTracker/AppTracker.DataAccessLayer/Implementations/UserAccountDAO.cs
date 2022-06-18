using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Contracts.Input;
using AppTracker.Models.Implementations.Responses;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class UserAccountDAO : IUserAccountDAO
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank {get;}
        public UserAccountDAO(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _options = options.Value;
            _messageBank = messageBank;
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

        public async Task<IResponse<string>> GetUserHashAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    connection.Open();
                    var procedure = "[GetUserHash]";
                    var parameters = new DynamicParameters();
                    parameters.Add("email", account.Email);
                    parameters.Add("Result", dbType: DbType.AnsiString, size: 128, direction: ParameterDirection.Output);
                    

                    var result = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedure, parameters,
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    return new Response<string>("success", parameters.Get<string>("Result"), 200, true);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case -1:
                        return new Response<string>("cannot connect to database", "-1", 503, false);
                    default:
                        return new Response<string>("unhandled exception" + ex.Message, "-1", 500, false);
                }

            }
            catch (OperationCanceledException ex)
            {
                return new Response<string>("cancellation requested", "-1", 500, false);
            }
            catch (Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "-1", 500, false);
            }
        }

        public async Task<IResponse<int>> VerifyAccountAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[VerifyAccount]";
                    var parameters = new DynamicParameters();
                    parameters.Add("Email", account.Email);
                    parameters.Add("AuthorizationLevel", account.AuthorizationLevel);
                    parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);

                    return new Response<int>("success", parameters.Get<int>("Result"), 200, true);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case -1:
                        return new Response<int>("cannot connect to database", 0, 503, false);
                    default:
                        return new Response<int>("unhandled exception" + ex.Message, 0, 500, false);
                }

            }
            catch (OperationCanceledException ex)
            {
                return new Response<int>("cancellation requested", 0, 500, false);
            }
            catch (Exception ex)
            {
                return new Response<int>("unhandled exception" + ex.Message, 0, 500, false);
            }
        }

        public async Task<IResponse<int>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[Authenticate]";
                    var parameters = new DynamicParameters();
                    parameters.Add("Email", authenticationInput.UserAccount.Email);
                    parameters.Add("Passphrase", authenticationInput.UserAccount.Password);
                    parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);

                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.authenticationSuccess);
                    return new Response<int>(messageResponse.Message, parameters.Get<int>("Result"), messageResponse.Code, true);

                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case -1:
                        {
                            IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.databaseConnectionFail);
                            return new Response<int>(messageResponse.Message, 0, messageResponse.Code, false);
                        }                        
                    default:
                        {
                            IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException);
                            return new Response<int>(messageResponse.Message + ex.Message, 0, messageResponse.Code, false);
                        }
                }

            }
            catch (OperationCanceledException ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.operationCancelled);
                return new Response<int>(messageResponse.Message + ex.Message, 0, messageResponse.Code, false);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException);
                return new Response<int>(messageResponse.Message + ex.Message, 0, messageResponse.Code, false);
            }
        }

    }
}
