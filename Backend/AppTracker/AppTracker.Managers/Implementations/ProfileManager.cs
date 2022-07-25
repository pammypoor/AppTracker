using AppTracker.Managers.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.Managers.Implementations
{
    public class ProfileManager: IProfileManager
    {
        private BuildSettingsOptions _options { get; }
        private IAuthenticationService _authenticationService { get; }
        private IProfileService _profileService { get; }
        private IMessageBank _messageBank { get; }
        public ProfileManager(IAuthenticationService authenticationService, IProfileService profileService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _authenticationService = authenticationService;
            _profileService = profileService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        public async Task<IResponse<IProfile>> GetProfileAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.notAuthenticated, cancellationToken);
                    return new Response<IProfile>(messageResponse.Message, null, messageResponse.Code, false);
                }

                cancellationToken.ThrowIfCancellationRequested();

                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    role = _options.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unknownRole, cancellationToken);
                    return new Response<IProfile>(messageResponse.Message, null, messageResponse.Code, false);
                }

                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                IUserAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Check if user is authorized to view profile
                IResponse<string> verifyAccountResult = await _authenticationService.VerifyAccountAsync(account, cancellationToken);
                if (verifyAccountResult.StatusCode != 200 && !verifyAccountResult.Data.Equals(_messageBank.GetMessageAsync(IMessageBank.Responses.accountVerificationSuccess).Result.Message))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.notAuthorized, cancellationToken);
                    return new Response<IProfile>(messageResponse.Message, null, messageResponse.Code, false);
                }

                IResponse<IProfile> getProfileResult = await _profileService.GetProfileAsync(userHash, cancellationToken);
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

        public async Task<IResponse<List<string>>> GetPronounsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<List<string>> getPronounsResult = await _profileService.GetPronounsAsync(cancellationToken);
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

        public async Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.notAuthenticated, cancellationToken);
                    return new Response<IProfile>(messageResponse.Message, profile, messageResponse.Code, false);
                }

                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    role = _options.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else 
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unknownRole, cancellationToken);
                    return new Response<IProfile>(messageResponse.Message, profile, messageResponse.Code, false);
                }

                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                IUserAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Check if user is authorized to make update to profile
                IResponse<string> verifyAccountResult = await _authenticationService.VerifyAccountAsync(account, cancellationToken);
                if (verifyAccountResult.StatusCode != 200 && !verifyAccountResult.Data.Equals(_messageBank.GetMessageAsync(IMessageBank.Responses.accountVerificationSuccess).Result.Message))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.notAuthorized, cancellationToken);
                    return new Response<IProfile>(messageResponse.Message, profile, messageResponse.Code, false);
                }

                IResponse<IProfile> updateProfileResult = await _profileService.UpdateProfileAsync(profile, userHash, cancellationToken);
                return updateProfileResult; 
            }
            catch (OperationCanceledException)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.operationCancelled, cancellationToken);
                return new Response<IProfile>(messageResponse.Message, profile, messageResponse.Code, false);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<IProfile>(messageResponse.Message + ex.Message, profile, messageResponse.Code, false);
            }
        }
    }
}
