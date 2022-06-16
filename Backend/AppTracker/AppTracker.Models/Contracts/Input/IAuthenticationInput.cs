using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.Models.Contracts.Input
{
    public interface IAuthenticationInput
    {
        public IUserAccount UserAccount { get; set; }
        string UserHash { get; set; }
    }
}
