using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using static AppTracker.MessageBank.Contracts.IMessageBank;

namespace AppTracker.MessageBank.Implementations
{
    public class MessageBank: IMessageBank
    {
        private BuildSettingsOptions _options { get; }

        public MessageBank(IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _options = options.Value;
        }
        public async Task<IMessageResponse> GetMessageAsync(Responses response, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (response)
            {
                case Responses.success:
                    return await GetMessageFromDataBaseAsync("success", cancellationToken);
                case Responses.unhandledException:
                    return await GetMessageFromDataBaseAsync("unhandledException", cancellationToken);
                case Responses.operationCancelled:
                    return await GetMessageFromDataBaseAsync("operationCancelled");
                case Responses.operationTimeExceeded:
                    return await GetMessageFromDataBaseAsync("operationTimeExceeded");
                case Responses.principalNotSet:
                    return await GetMessageFromDataBaseAsync("principalNotSet");
                case Responses.unknownRole:
                    return await GetMessageFromDataBaseAsync("unknownRole");
                case Responses.databaseConnectionFail:
                    return await GetMessageFromDataBaseAsync("databaseConnectionFail");
                case Responses.accountVerificationSuccess:
                    return await GetMessageFromDataBaseAsync("accountVerificationSuccess");
                case Responses.accountVerificationFail:
                    return await GetMessageFromDataBaseAsync("accountVerificationFail");
                case Responses.accountNotEnabled:
                    return await GetMessageFromDataBaseAsync("accountNotEnabled");
                case Responses.accountNotConfirmed:
                    return await GetMessageFromDataBaseAsync("accountNotConfirmed");
                case Responses.accountNotFound:
                    return await GetMessageFromDataBaseAsync("accountNotFound");
                case Responses.authenticationSuccess:
                    return await GetMessageFromDataBaseAsync("authenticationSuccess");
                case Responses.getUserHashSuccess:
                    return await GetMessageFromDataBaseAsync("getUserHashSuccess");
                case Responses.invalidPassword:
                    return await GetMessageFromDataBaseAsync("invalidPassword");
                case Responses.tokenRefreshSuccess:
                    return await GetMessageFromDataBaseAsync("tokenRefreshSuccess");
                case Responses.alreadyAuthenticated:
                    return await GetMessageFromDataBaseAsync("alreadyAuthenticated");
                case Responses.notAuthenticated:
                    return await GetMessageFromDataBaseAsync("notAuthenticated");
                case Responses.notAuthorized:
                    return await GetMessageFromDataBaseAsync("notAuthorized");
                case Responses.invalidApplication:
                    return await GetMessageFromDataBaseAsync("invalidApplication");
                default:
                    return new MessageResponse("Error retrieving error code", 500);
            }
        }


        public async Task<IMessageResponse> GetMessageFromDataBaseAsync(string response, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    connection.Open();
                    var procedure = "[GetMessage]";
                    var parameters = new DynamicParameters();
                    parameters.Add("Response", response);
                    parameters.Add("ResultMessage", dbType: DbType.AnsiString, size: 150, direction: ParameterDirection.Output);
                    parameters.Add("ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);

                    return new MessageResponse(parameters.Get<string>("ResultMessage"), parameters.Get<int>("ResultCode"));
                }
            } 
            catch(Exception ex)
            {
                return new MessageResponse(ex.Message, 500);
            }
        }
    }


}
