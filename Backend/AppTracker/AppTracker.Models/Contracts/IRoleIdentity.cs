using System.Security.Principal;


namespace AppTracker.Models.Contracts
{
    public interface IRoleIdentity: IIdentity
    {
        string AuthorizationLevel { get; }
        string UserHash { get; }
    }
}
