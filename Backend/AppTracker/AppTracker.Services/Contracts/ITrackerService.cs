using AppTracker.Models.Contracts;

namespace AppTracker.Services.Contracts
{
    public interface ITrackerService
    {
        public Task<IResponse<string>> CreateApplicationAsync(IApplication application, CancellationToken cancellationToken = default(CancellationToken));
    }
}
