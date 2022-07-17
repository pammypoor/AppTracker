using AppTracker.Models.Contracts;
using AppTracker.Models.Contracts.Input;

namespace AppTracker.DataAccessLayer.Contracts
{
    public interface IAuthenticationDAO
    {
        public Task<IResponse<string>> CreateUserAccountAsync(IUserAccount account, string userHash, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<string>> GetUserHashAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<int>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<int>> VerifyAccountAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken));
    }
}
