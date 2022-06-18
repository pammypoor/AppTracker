using AppTracker.Models.Contracts;

namespace AppTracker.Managers.Contracts
{
    public interface ITrackerManager
    {
        public Task<IResponse<string>> CreateApplicationAsync(IApplication application, CancellationToken cancellationToken = default(CancellationToken));
    }
}
