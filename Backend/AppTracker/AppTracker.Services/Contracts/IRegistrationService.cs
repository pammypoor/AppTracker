using AppTracker.Models.Contracts;

namespace AppTracker.Services.Contracts
{
    public interface IRegistrationService
    {
        public Task<IResponse<string>> CreateUserAccountAsync(string email, string password, string authorizationLevel, bool enabled, bool confirmed, CancellationToken cancellationToken = default(CancellationToken));
    }
}
