

namespace AppTracker.Models.Contracts
{
    public interface IUserAccount
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
    }
}
