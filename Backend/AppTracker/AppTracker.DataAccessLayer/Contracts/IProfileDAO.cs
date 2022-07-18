using AppTracker.Models.Contracts;

namespace AppTracker.DataAccessLayer.Contracts
{
    public interface IProfileDAO
    {
        public Task<IResponse<IProfile>> GetProfileAsync(string userHash, CancellationToken cancellationToken = default(CancellationToken));
        public Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile, string userHash, CancellationToken cancellationToken = default(CancellationToken));
    }
}
