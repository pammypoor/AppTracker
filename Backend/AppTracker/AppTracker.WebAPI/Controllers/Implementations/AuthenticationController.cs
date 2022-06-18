using AppTracker.Managers.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.WebAPI.Controllers.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppTracker.WebAPI.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class AuthenticationController: ControllerBase, IAuthenticationController
    {
        private IAuthenticationManager _authenticationManager { get; }
        private BuildSettingsOptions _options { get; }

        public AuthenticationController(IAuthenticationManager authenticationManager, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _authenticationManager=authenticationManager;
            _options=buildSettingsOptions.Value;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(string email, string password)
        {
            try
            {
                IResponse<string> result = await _authenticationManager.AuthenticateAsync(email, password, _options.User).ConfigureAwait(false);

                return StatusCode(result.StatusCode, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
