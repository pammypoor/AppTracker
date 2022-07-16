using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.WebAPI.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppTracker.WebAPI.Controllers.Implementations
{
    public class ProfileController: ControllerBase, IProfileController
    {
        private  ProfileManager _profileManager { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }

        public ProfileController(IProfileManager profileManager, IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _profileManager = profileManager;
            _options = options.Value;
            _messageBank = messageBank;
        }
        [HttpPost("createApplication")]
        public async Task<IActionResult> Update()
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
