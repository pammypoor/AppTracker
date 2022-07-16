using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class ProfileDAO: IProfileDAO
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        public ProfileDAO(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _options = options.Value;
            _messageBank = messageBank;
        } 

        public async Task<IResponse<IProfile>> UpdateProfileAsync(IProfile profile)
        {

        }
    }
}
