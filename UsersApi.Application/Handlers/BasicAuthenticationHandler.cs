using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using UsersApi.Application.Services.Authentication;

namespace UsersApi.Application.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticateService _authenticateService;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthenticateService authenticateService) : base(options, logger, encoder, clock)
        {
            _authenticateService = authenticateService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");

            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var parameter = authenticationHeaderValue.Parameter;
            if (parameter == null)
                return AuthenticateResult.Fail("Authorization credentials are null");
            var bytes = Convert.FromBase64String(parameter);
            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
            string login = credentials[0];
            string password = credentials[1];

            try
            {
                var user = await _authenticateService.Authenticate(login, password);
                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserGroup.Code.ToString())
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Incorrect login or password");
            }
        }
    }
}
