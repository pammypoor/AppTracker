using AppTracker.Models.Contracts;

namespace AppTracker.Models.Implementations
{
    public class RoleIdentity : IRoleIdentity
    {
        public string AuthenticationType => "AuthorizationLevel";
        public bool IsAuthenticated { get; }
        public string Name { get;  }
        public string AuthorizationLevel { get; }
        public string UserHash { get; }
        public RoleIdentity (bool isAuthenticated, string name, string authorizationLevel, string userHash)
        {
            IsAuthenticated = isAuthenticated;
            Name = name;
            AuthorizationLevel = authorizationLevel;
            UserHash = userHash;
        }
    }
}
