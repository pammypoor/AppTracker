using AppTracker.Managers.Contracts;
using AppTracker.Managers.Implementations;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations;
using AppTracker.WebAPI.Controllers.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace AppTracker.WebAPI.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class ProfileController: ControllerBase, IProfileController
    {
        private  IProfileManager _profileManager { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }

        public ProfileController(IProfileManager profileManager, IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _profileManager = profileManager;
            _options = options.Value;
            _messageBank = messageBank;
        }

        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateProfileAsync(Profile profile)
        {
            try
            {
                IResponse<IProfile> result = await _profileManager.UpdateProfileAsync(profile);

                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException);
                return StatusCode(messageResponse.Code, messageResponse.Message);
            }
        }

        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            try
            {
                IResponse<IProfile> result = await _profileManager.GetProfileAsync();
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
