using AppTracker.Managers.Contracts;
using AppTracker.Managers.Implementations;
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
        private  IProfileManager _profileManager { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }

        public ProfileController(IProfileManager profileManager, IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _profileManager = profileManager;
            _options = options.Value;
            _messageBank = messageBank;
        }
    }
}
