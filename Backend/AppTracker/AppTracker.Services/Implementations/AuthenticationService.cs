using AppTracker.DataAccessLayer.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.Services.Implementations
{
    public class AuthenticationService: IAuthenticationService
    {
        private IUserAccountDAO _userAccountDAO { get; }
        private BuildSettingsOptions _options { get; }
        private string _payLoad { get; }

        public AuthenticationService(IUserAccountDAO userAccountDAO, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _userAccountDAO = userAccountDAO;
            _options = options.Value;
            _payLoad = "";
        }

        public async Task<IResponse<string>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken))
        {

        }


    }
}
