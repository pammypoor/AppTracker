using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.Services.Implementations
{
    public class AuthorizationService: IAuthorizationService
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        private IApplicationDAO _applicationDAO { get; }
        private IAuthorizationDAO _userAccountDAO { get; }
        public AuthorizationService(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank, IApplicationDAO applicationDAO)
        {
            _options = options.Value;
            _messageBank = messageBank;
            _applicationDAO = applicationDAO;
        }

        public async Task<IResponse<bool>> UserAuthorizedToChangeAsync(string userHash, IApplication application, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.success, cancellationToken);
                return  new Response<bool>(messageResponse.Message, userHash.Equals(application.UserHash), messageResponse.Code, true);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<bool>(messageResponse.Message, false, messageResponse.Code, false);
            }
        }
    }
}
