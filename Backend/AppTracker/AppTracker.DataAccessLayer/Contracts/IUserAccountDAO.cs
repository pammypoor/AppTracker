using AppTracker.Models.Contracts;

namespace AppTracker.DataAccessLayer.Contracts
{
    public interface IUserAccountDAO
    {
        public Task<IResponse<string>> CreateUserAccountAsync(IUserAccount account, string userHash, CancellationToken cancellationToken = default(CancellationToken));
    }
}
