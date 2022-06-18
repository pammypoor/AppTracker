using AppTracker.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.DataAccessLayer.Contracts
{
    public interface IApplicationDAO
    {
        public Task<IResponse<string>> CreateApplicationAsync(IApplication application, CancellationToken cancellationToken = default(CancellationToken));
    }
}
