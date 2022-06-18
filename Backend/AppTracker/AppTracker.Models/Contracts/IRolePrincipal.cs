using System.Security.Principal;

namespace AppTracker.Models.Contracts
{
    public interface IRolePrincipal: IPrincipal
    {
        public IRoleIdentity RoleIdentity { get; }
    }
}
