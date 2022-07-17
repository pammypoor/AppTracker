using AppTracker.Models.Contracts;

namespace AppTracker.Services.Contracts
{
    public interface IProfileService
    {
        public Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile, string userHash, CancellationToken cancellationToken = default(CancellationToken));
    }
}
