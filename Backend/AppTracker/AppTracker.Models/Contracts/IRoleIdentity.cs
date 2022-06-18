using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.Models.Contracts
{
    public interface IRoleIdentity
    {
        string AuthorizationLevel { get; }
        string UserHash { get; }
    }
}
