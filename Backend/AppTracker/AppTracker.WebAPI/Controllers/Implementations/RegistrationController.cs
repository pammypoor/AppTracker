using AppTracker.Managers.Contracts;
using AppTracker.Models.Contracts;
using AppTracker.WebAPI.Controllers.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AppTracker.WebAPI.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class RegistrationController: ControllerBase, IRegistrationController
    {
        private IRegistrationManager _registrationManager { get;  }

        public RegistrationController(IRegistrationManager registrationmanager)
        {
            _registrationManager = registrationmanager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccountAsync(string email, string password)
        {
            try
            {
                IResponse<string> result = await _registrationManager.CreateUserAccountAsync(email, password).ConfigureAwait(false);

                return StatusCode(result.StatusCode, result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
