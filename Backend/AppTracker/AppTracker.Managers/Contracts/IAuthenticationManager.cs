using AppTracker.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.Managers.Contracts
{
    public interface IAuthenticationManager
    {
        public Task<IResponse<string>> AuthenticateAsync(string email, string password, string authenticationLevel, CancellationToken cancellationToken = default(CancellationToken));
    }
}
