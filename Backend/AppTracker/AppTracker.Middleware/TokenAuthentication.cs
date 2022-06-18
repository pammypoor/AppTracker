using AppTracker.Managers.Contracts;
using AppTracker.MessageBank.Contracts;
using AppTracker.Models;
using AppTracker.Models.Contracts;
using AppTracker.Models.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AppTracker.Middleware
{
    public class TokenAuthentication
    {
        private RequestDelegate _next { get; }
        private IOptionsMonitor<BuildSettingsOptions> _options { get; }
        private IMessageBank _messageBank { get; }
        public TokenAuthentication(RequestDelegate next, IOptionsMonitor<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _next = next;
            _options = options;
            _messageBank = messageBank;
        }

        public async Task Invoke (HttpContext httpContext, IMessageBank messageBank, IAuthenticationManager authenticationManager)
        {
            try
            {
                if (!httpContext.Request.Headers.ContainsKey(_options.CurrentValue.JWTHeaderName) || httpContext.Request.Headers[_options.CurrentValue.JWTHeaderName].Equals("null"))
                {
                    IRoleIdentity guestIdentity = new RoleIdentity(true, _options.CurrentValue.GuestName, _options.CurrentValue.GuestRole, _options.CurrentValue.GuestHash);
                    IRolePrincipal rolePrincipal = new RolePrincipal(guestIdentity);
                    Thread.CurrentPrincipal = rolePrincipal;
                } else
                {
                    string jwt = httpContext.Request.Headers[_options.CurrentValue.JWTHeaderName];
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var keyValue = _options.CurrentValue.JWTTokenKey;
                    var key = Encoding.UTF8.GetBytes(keyValue);
                    tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _options.CurrentValue.JwtIssuer,
                        ValidateAudience = false,
                        ValidAlgorithms = new [] {_options.CurrentValue.JwtHashAlgorithm},
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                    var jwtToken = (JwtSecurityToken)validatedToken;
                    
                    RoleIdentity roleIdentity = new RoleIdentity(true,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier1).Value,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier2).Value,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier3).Value);
                    IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                    Thread.CurrentPrincipal = rolePrincipal;

                }
            }
            catch(Exception ex)
            {
                IRoleIdentity guestIdentity = new RoleIdentity(true, _options.CurrentValue.GuestName, _options.CurrentValue.GuestRole, _options.CurrentValue.GuestHash);
                IRolePrincipal rolePrincipal = new RolePrincipal(guestIdentity);
                Thread.CurrentPrincipal = rolePrincipal;
            } 
            finally
            {
                await _next(httpContext);
            }

        }
    }
}
