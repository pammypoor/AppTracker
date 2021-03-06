
using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;

namespace AppTracker.Services.Implementations
{
    public class TrackerService: ITrackerService
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        private IApplicationDAO _applicationDAO { get; }
        public TrackerService(IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank, IApplicationDAO applicationDAO)
        {
            _options = options.Value;
            _messageBank = messageBank;
            _applicationDAO = applicationDAO;
        }
        public async Task<IResponse<string>> CreateApplicationAsync(IApplication application, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _applicationDAO.CreateApplicationAsync(application, cancellationToken);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<string>(messageResponse.Message, messageResponse.Message + ex.Message, messageResponse.Code, false);
            }
        }
    }
}
