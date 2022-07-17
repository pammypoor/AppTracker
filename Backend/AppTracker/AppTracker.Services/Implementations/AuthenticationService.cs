using AppTracker.DataAccessLayer.Contracts;
using AppTracker.MessageBank.Contracts;
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
        private IAuthenticationDAO _userAccountDAO { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }

        public AuthenticationService(IAuthenticationDAO userAccountDAO, IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _userAccountDAO = userAccountDAO;
            _options = options.Value;
            _messageBank = messageBank;
        }

        public async Task<IResponse<string>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                IResponse<string> getUserHashResult = await _userAccountDAO.GetUserHashAsync(authenticationInput.UserAccount);

                if(!getUserHashResult.IsSuccess)
                {
                    return getUserHashResult;
                }

                authenticationInput.UserHash = getUserHashResult.Data;

                string token = await CreateJwtTokenAsync(authenticationInput, cancellationToken).ConfigureAwait(false);

                IResponse<int> authenticateResult = await _userAccountDAO.AuthenticateAsync(authenticationInput, cancellationToken).ConfigureAwait(false);

                
                switch (authenticateResult.Data)
                {
                    case 1:
                        return new Response<string>(authenticateResult.ErrorMessage, token, authenticateResult.StatusCode, true);
                    default:
                        {
                            // Not correct
                            IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.invalidPassword, cancellationToken);
                            return new Response<string>(messageResponse.Message, messageResponse.Message, messageResponse.Code, false);
                        }
                };
            }
            catch(Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<string>(messageResponse.Message, messageResponse.Message + ex.Message, messageResponse.Code, false);
            }
        }

        public async Task<IResponse<string>> VerifyAccountAsync(IUserAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<int> result = await _userAccountDAO.VerifyAccountAsync(account, cancellationToken);
                
                IMessageResponse messageResponse;
                switch (result.Data)
                {
                    case 0:
                        messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.accountNotFound);
                        break;
                    case 1:
                        messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.accountVerificationSuccess);
                        break;
                    case 2:
                        messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.accountNotEnabled);
                        break;
                    case 3:
                        messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.accountNotConfirmed);
                        break;
                    default:
                        messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.accountVerificationFail);
                        break;
                }

                if(result.StatusCode == 200)
                {
                    return new Response<string>(messageResponse.Message, messageResponse.Message, result.StatusCode, true);
                } 
                else
                {
                    return new Response<string>(messageResponse.Message, messageResponse.Message, result.StatusCode, false);
                }
            }
            catch (Exception ex)
            {
                return new Response<string>("unhandled exception" + ex.Message, "", 500, false);
            }
        }

        public async Task<IResponse<string>> RefreshSessionAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string token = await CreateJwtTokenAsync(authenticationInput, cancellationToken);
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.tokenRefreshSuccess, cancellationToken);
                return new Response<string>(messageResponse.Message, token, messageResponse.Code, true);
            }
            catch (Exception ex)
            {
                IMessageResponse messageResponse = await _messageBank.GetMessageAsync(IMessageBank.Responses.unhandledException, cancellationToken);
                return new Response<string>(messageResponse.Message, messageResponse.Message + ex.Message, messageResponse.Code, false);
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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
