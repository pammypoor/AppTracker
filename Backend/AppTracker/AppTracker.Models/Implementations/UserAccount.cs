using AppTracker.Models.Contracts;

namespace AppTracker.Models.Implementations
{
    public class UserAccount: IUserAccount, IEquatable<UserAccount>
    {
        public long UserAccountID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AuthorizationLevel { get; set; }
        public bool Enabled { get; set; }
        public bool Confirmed { get; set; }
        public string Token { get; set; }

        public bool Equals(UserAccount? obj)
        {
            if (obj != null)
            {
                return UserAccountID.Equals(obj.UserAccountID);
            }
            return false;
        }

        public UserAccount(string Email, string Password, string AuthorizationLevel, bool Enabled, bool Confirmed)
        {
            this.Email = Email;
            this.Password = Password;
            this.AuthorizationLevel = AuthorizationLevel;
            this.Enabled = Enabled;
            this.Confirmed = Confirmed;
        }

        public UserAccount(string Email, string Password, string AuthorizationLevel)
        {
            this.Email = Email;
            this.Password = Password;
            this.AuthorizationLevel = AuthorizationLevel;
        }
    }
}
