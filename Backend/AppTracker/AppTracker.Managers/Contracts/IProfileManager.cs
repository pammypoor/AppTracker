using AppTracker.Models.Contracts;

namespace AppTracker.Managers.Contracts
{
    public interface IProfileManager
    {
        public Task<IResponse<IProfile>> GetProfileAsync(CancellationToken cancellationToken = default(CancellationToken));
        public Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile, CancellationToken cancellationToken = default(CancellationToken));
    }
}
