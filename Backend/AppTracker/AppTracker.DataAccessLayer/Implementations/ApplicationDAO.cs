using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations.Responses;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class ApplicationDAO: IApplicationDAO
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        public ApplicationDAO(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _options = options.Value;
            _messageBank = messageBank;
        }

        public async Task<IResponse<string>> CreateApplicationAsync(IApplication application, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    connection.Open();
                    var procedure = "[CreateApplication]";
                    var parameters = new DynamicParameters();
                    parameters.Add("UserHash", application.UserHash);
                    parameters.Add("SubmissionDateTime", application.SubmissionDateTime);
                    parameters.Add("Company", application.Company);
                    parameters.Add("Position", application.Position);
                    parameters.Add("Type", application.Type);
                    parameters.Add("Status", application.Status);
                    parameters.Add("Link", application.Link);
                    parameters.Add("State", application.State);
                    parameters.Add("City", application.City);
                    parameters.Add("Country", application.Country);
                    parameters.Add("Description", application.Description);
                    parameters.Add("IsRemote", application.IsRemote);
                    parameters.Add("Deleted", application.Deleted);


                    var result = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedure, parameters,
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.getUserHashSuccess);
                    return new Response<string>(messageResponse.Message, parameters.Get<string>("Result"), messageResponse.Code, true);
                }
            }
        }
    }
}
