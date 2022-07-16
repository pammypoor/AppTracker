using AppTracker.DataAccessLayer.Contracts;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using System.Security.Cryptography;

namespace AppTracker.Services.Implementations
{
    public class RegistrationService: IRegistrationService
    {
        private IAuthorizationDAO _DAO { get; set; }

        public RegistrationService(IAuthorizationDAO DAO)
        {
            _DAO = DAO;
        }

        public async Task<IResponse<string>> CreateUserAccountAsync(string email, string password, string authorizationLevel, bool enabled, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                IUserAccount account = new UserAccount(email, password, authorizationLevel, enabled, confirmed);
                
                string accountHash = await GetHashValueAsync(account.Email);

                IResponse<string> result = await _DAO.CreateUserAccountAsync(account, accountHash, cancellationToken);

                return result;
            }
            catch (OperationCanceledException ex)
            {
                return new Response<string>("cancellation requested", "", 500, false);
            }
            catch (Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "", 500, false);
            }
        }

        private async Task<string> GetHashValueAsync(string value)
        {
            try
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(value, 0, iterations: 10000, HashAlgorithmName.SHA512);
                return string.Join(string.Empty, Array.ConvertAll(pbkdf2.GetBytes(64), b => b.ToString("X2")));
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}
