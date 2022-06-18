using AppTracker.DataAccessLayer.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Contracts.Input;
using AppTracker.Models.Implementations;
using AppTracker.Models.Implementations.Responses;
using AppTracker.Services.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppTracker.Services.Implementations
{
    public class AuthenticationService: IAuthenticationService
    {
        private IUserAccountDAO _userAccountDAO { get; }
        private BuildSettingsOptions _options { get; }
        private string _payLoad { get; }

        public AuthenticationService(IUserAccountDAO userAccountDAO, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _userAccountDAO = userAccountDAO;
            _options = options.Value;
            _payLoad = "";
        }

        public async Task<IResponse<string>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                IResponse<string> GetUserHashResult = await _userAccountDAO.GetUserHashAsync(authenticationInput.UserAccount);
                authenticationInput.UserHash = GetUserHashResult.Data;

                string token = await CreateJwtTokenAsync(authenticationInput, cancellationToken).ConfigureAwait(false);

                IResponse<int> authenticateResult = await _userAccountDAO.AuthenticateAsync(authenticationInput, cancellationToken).ConfigureAwait(false);

                switch (authenticateResult.Data)
                {
                    case 1:
                        return new Response<string>(authenticateResult.ErrorMessage, authenticateResult.ErrorMessage, authenticateResult.StatusCode, true);
                            break;
                    default:
                        // Not correct
                        throw new Exception(authenticateResult.Data.ToString());
                };
            }
            catch(Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "-1", 500, false);
            }
        }

        public async Task<IResponse<string>> VerifyAccountAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<int> result = await _userAccountDAO.VerifyAccountAsync(account, cancellationToken);
                switch (result.Data)
                {
                    case 0:
                        return new Response<string>("Account Not Found", "Account Not Found", 404, false);
                    case 1:
                        return new Response<string>("success", "", 200, true);
                    case 2:
                        return new Response<string>("Account Disabled", "Account Disabled", 403, false);
                    case 3:
                        return new Response<string>("Account Unconfirmed", "Account Unconfirmed", 403, false);
                    default:
                        return new Response<string>("Error Validating", "Error Validating", 500, false);
                }
            }
            catch (Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "", 500, false);
            }
        }

        private async Task<string> CreateJwtTokenAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                Dictionary<string, string> claimValuePairs = new Dictionary<string, string>();
                claimValuePairs.Add(_options.RoleIdentityIdentifier1, authenticationInput.UserAccount.Email);
                claimValuePairs.Add(_options.RoleIdentityIdentifier2, authenticationInput.UserAccount.AuthorizationLevel);
                claimValuePairs.Add(_options.RoleIdentityIdentifier3, authenticationInput.UserHash);

                IRoleIdentity roleIdentity = new RoleIdentity(true, claimValuePairs[_options.RoleIdentityIdentifier1], claimValuePairs[_options.RoleIdentityIdentifier2], claimValuePairs[_options.RoleIdentityIdentifier3]);

                var tokenHandler = new JwtSecurityTokenHandler();
                var keyValue = _options.JWTTokenKey;
                var key = Encoding.UTF8.GetBytes(keyValue);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(_options.RoleIdentityIdentifier1,
                            claimValuePairs[_options.RoleIdentityIdentifier1]),
                        new Claim(_options.RoleIdentityIdentifier2,
                            claimValuePairs[_options.RoleIdentityIdentifier2]),
                        new Claim(_options.RoleIdentityIdentifier3,
                            claimValuePairs[_options.RoleIdentityIdentifier3])}),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    IssuedAt = DateTime.UtcNow,
                    Issuer = _options.JwtIssuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ricf)
            {
                return ricf.Message;
            }
        }
    }
}
