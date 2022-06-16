using AppTracker.Models;
using Microsoft.Extensions.Options;

namespace AppTracker.DataAccessLayer.Implementations
{
    public class SqlDAO
    {
        private BuildSettingsOptions _options { get;  }

        public SqlDAO(IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _options = options.Value;
        }


    }
}
