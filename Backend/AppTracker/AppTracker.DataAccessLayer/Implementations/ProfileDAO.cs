﻿using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations;
using AppTracker.Models.Implementations.Responses;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class ProfileDAO: IProfileDAO
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        public ProfileDAO(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _options = options.Value;
            _messageBank = messageBank;
        } 

        public async Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    connection.Open();
                    var procedure = "[UpdateProfile]";
                    var parameters = new
                    {
                        @UserHash = userHash,
                        @Pronoun = profile.Pronouns,
                        @Position = profile.Position,
                        @Company = profile.Company,
                        @Degree = profile.Degree,
                        @School = profile.School,
                        @Field = profile.Field,
                        @GraduationDate = profile.GraduationDate,
                        @LocationCity = profile.LocationCity,
                        @LocationCountry = profile.LocationCountry,
                        @About = profile.About
                    };

                    var result = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedure, parameters,
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    return new Response<IProfile>("success", profile, 200, true);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case -1:
                        return new Response<IProfile>("cannot connect to database", profile, 503, false);
                    default:
                        return new Response<IProfile>("unhandled exception" + ex.Message, profile, 500, false);
                }

            }
            catch (OperationCanceledException ex)
            {
                return new Response<IProfile>("cancellation requested", profile, 500, false);
            }
            catch (Exception ex)
            {
                return new Response<IProfile>("unhandled exception" + ex.Message, profile, 500, false);
            }
        }
    }
}
