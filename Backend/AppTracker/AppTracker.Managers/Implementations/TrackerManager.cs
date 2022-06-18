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
    public class TrackerManager: ITrackerManager
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        private ITrackerService _trackerService { get; }
        private IAuthenticationService _authenticationService { get; }

        private IAuthorizationService _authorizationService { get; }
        public TrackerManager(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank, ITrackerService trackerService, IAuthenticationService authenticationService, IAuthorizationService authorizationService)
        {
            _options = options.Value;
            _messageBank = messageBank;
            _trackerService = trackerService;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
        }
        public async Task<IResponse<string>> CreateApplicationAsync(IApplication application, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    return new Response<string>(messageResponse.Message, "", messageResponse.Code, false); 
                }

                if(application == null || application.Position.Equals("") || application.Company.Equals(""))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.invalidApplication).ConfigureAwait(false);
                    return new Response<string>(messageResponse.Message, "", messageResponse.Code, false);
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
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unknownRole).ConfigureAwait(false);
                    return new Response<string>(messageResponse.Message, "", messageResponse.Code, false);
                }

                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                IUserAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                IResponse<string> resultVerifyAccount = await _authenticationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.IsSuccess)
                {
                    return resultVerifyAccount;
                }

                IResponse<bool> verifyCanMakeChanges = await _authorizationService.UserAuthorizedToChangeAsync(userHash, application);

                if (!verifyCanMakeChanges.Data)
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                    return new Response<string>(messageResponse.Message, "", messageResponse.Code, false);
                }

                IResponse<string> result = await _trackerService.CreateApplicationAsync(application, cancellationToken).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<string>(messageResponse.Message, messageResponse.Message + ex.Message, messageResponse.Code, false);
            }
        }
    }
}
