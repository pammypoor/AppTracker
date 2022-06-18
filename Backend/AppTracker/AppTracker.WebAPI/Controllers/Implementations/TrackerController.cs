using AppTracker.Managers.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations;
using AppTracker.WebAPI.Controllers.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppTracker.WebAPI.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class TrackerController: ControllerBase, ITrackerController
    {
        private ITrackerManager _trackerManager { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }

        public TrackerController(ITrackerManager trackerManager, IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _trackerManager = trackerManager;
            _options = options.Value;
            _messageBank = messageBank;
        }

        [HttpPost("createApplication")]
        public async Task<IActionResult> AuthenticateAsync(Application application)
        {
            try
            {
                IResponse<string> result = await _trackerManager.CreateApplicationAsync(application).ConfigureAwait(false);

                return StatusCode(result.StatusCode, result.Data);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException);
                return StatusCode(messageResponse.Code, messageResponse.Message);
            }
        }
    }
}
