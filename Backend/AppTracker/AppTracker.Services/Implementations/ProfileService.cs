using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.Services.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfileService: IProfileService
    {
        private IProfileDAO _profileDAO { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        public ProfileService(IProfileDAO profileDAO, IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _profileDAO = profileDAO;
            _options = options.Value;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     Updates user profile using DAO
        /// </summary>
        /// <param name="profile">User profile</param>
        /// <param name="userHash">User hash</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Response</returns>
        public async Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                IResponse<IProfile> updateProfileResult = await _profileDAO.UpdateProfileAsync(profile, userHash, cancellationToken);

                return updateProfileResult;
            }
            catch(OperationCanceledException)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.operationCancelled, cancellationToken);
                return new Response<IProfile>(messageResponse.Message, profile, messageResponse.Code, false);
            }
            catch(Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<IProfile>(messageResponse.Message + ex.Message, profile, messageResponse.Code, false);
            }
        }


        public async Task<IResponse<IProfile>> GetProfileAsync(string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<IProfile> getProfileResult = await _profileDAO.GetProfileAsync(userHash, cancellationToken);
                return getProfileResult;
            }
            catch (OperationCanceledException)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.operationCancelled, cancellationToken);
                return new Response<IProfile>(messageResponse.Message, null, messageResponse.Code, false);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<IProfile>(messageResponse.Message + ex.Message, null, messageResponse.Code, false);
            }
        }

        /// <summary>
        /// Retrieves a list of pronouns retrieved from database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResponse<List<string>>> GetPronounsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<List<string>> getPronounsResult = await _profileDAO.GetPronounsAsync(cancellationToken);
                return getPronounsResult;
            }
            catch (OperationCanceledException)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.operationCancelled, cancellationToken);
                return new Response<List<string>>(messageResponse.Message, null, messageResponse.Code, false);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<List<string>>(messageResponse.Message + ex.Message, null, messageResponse.Code, false);
            }
        }
    }
}
