using AppTracker.Models.Contracts;
using AppTracker.Models.Contracts.Input;

namespace AppTracker.Services.Contracts
{
    public interface IAuthenticationService
    {
        public Task<IResponse<string>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<string>> VerifyAccountAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken));
    }
}
