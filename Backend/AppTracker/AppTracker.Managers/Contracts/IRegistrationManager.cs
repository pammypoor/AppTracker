using AppTracker.Models.Contracts;

namespace AppTracker.Managers.Contracts
{
    public interface IRegistrationManager
    {
        public Task<IResponse<string>> CreateUserAccountAsync(string email, string password, CancellationToken cancellationToken = default(CancellationToken));
    }
}
