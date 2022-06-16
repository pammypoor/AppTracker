using AppTracker.Models.Contracts;
using AppTracker.Models.Contracts.Input;

namespace AppTracker.Models.Implementations.Input
{
    public class AuthenticationInput: IAuthenticationInput
    {
        public IUserAccount UserAccount { get; set; }
        public string UserHash { get; set; }

        public AuthenticationInput(IUserAccount account, string userHash)
        {
            UserAccount = account;
            UserHash = userHash;
        }
    }
}
