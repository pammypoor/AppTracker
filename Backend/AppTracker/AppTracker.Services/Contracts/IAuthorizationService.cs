
using AppTracker.Models.Contracts;

namespace AppTracker.Services.Contracts
{
    public interface IAuthorizationService
    {
        public Task<IResponse<bool>> UserAuthorizedToChangeAsync(string userHash, IApplication application, CancellationToken cancellationToken = default(CancellationToken));
    }
}
