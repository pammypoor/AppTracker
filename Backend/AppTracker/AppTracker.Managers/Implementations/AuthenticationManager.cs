using AppTracker.Managers.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models.Contracts;
using AppTracker.Models.Contracts.Input;
using AppTracker.Models.Implementations;
using AppTracker.Models.Implementations.Input;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;

namespace AppTracker.Managers.Implementations
{
    public class AuthenticationManager: IAuthenticationManager
    {
        private IAuthenticationService _authenticationService { get; }

        private IMessageBank _messageBank { get; }
        public AuthenticationManager(IAuthenticationService authenticationService, IMessageBank messageBank)
        {
            _authenticationService = authenticationService;
            _messageBank = messageBank;
        }

        public async Task<IResponse<string>> AuthenticateAsync(string email, string password, string authenticationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.alreadyAuthenticated, cancellationToken);
                    return new Response<string>(messageResponse.Message, "", messageResponse.Code, false);
                }

                IUserAccount account = new UserAccount(email, password, authenticationLevel);

                IResponse<string> VerifyAccountResult = await _authenticationService.VerifyAccountAsync(account, cancellationToken);

                if (!VerifyAccountResult.IsSuccess)
                {
                    return VerifyAccountResult;
                }

                IResponse<string> AuthenticateResult = await _authenticationService.AuthenticateAsync(new AuthenticationInput(account), cancellationToken);

                return AuthenticateResult;

            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<string>(messageResponse.Message, messageResponse.Message + ex.Message, messageResponse.Code, false);
            }
        }

        public async Task<IResponse<string>> RefreshSessionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                IUserAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, (Thread.CurrentPrincipal.Identity as IRoleIdentity).AuthorizationLevel);
                IAuthenticationInput authenticationInput = new AuthenticationInput(account,
                        (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash);
                return await _authenticationService.RefreshSessionAsync(authenticationInput, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<string>(messageResponse.Message, messageResponse.Message + ex.Message, messageResponse.Code, false);
            }
        }

    }
}
