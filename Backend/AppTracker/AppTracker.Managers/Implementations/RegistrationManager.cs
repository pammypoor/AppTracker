using AppTracker.Managers.Contracts
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.Managers.Implementations
{
    public class RegistrationManager: IRegistrationManager
    {
        private BuildSettingsOptions _options { get; }
        private IRegistrationService _registrationService { get; set; }

        public RegistrationManager(IRegistrationService registrationService, IOptions<BuildSettingsOptions> options)
        {
            _registrationService = registrationService;
            _options = options.Value;
        }

        public async Task<IResponse<string>> CreateUserAccountAsync(string email, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                IResponse<string> result = await _registrationService.CreateUserAccountAsync(email, password, _options.User, true, true);

                return result;
            }
            catch (OperationCanceledException ex)
            {
                return new Response<string>("cancellation requested", "", 500, false);
            }
            catch (Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "", 500, false);
            }
        }
    }
}
