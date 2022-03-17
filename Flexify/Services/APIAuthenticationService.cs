using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Flexify.Models;
using Flexify.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flexify.Services
{
    public class APIAuthenticationService: AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticationRepository _repository;


        public APIAuthenticationService(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IAuthenticationRepository repository) 
            : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
                     //exclude anonymous endpoints from the authentication rule
            Endpoint endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            //Authorization: <auth scheme> : <auth string>
            if (!Request.Headers.ContainsKey("Authorization"))
                throw new Exception("Authorization header required!");

            AuthenticationHeaderValue authorizationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            string key = authorizationHeader.Parameter;

            Auth auth = await _repository.Authenticate(key);
            if(auth is null)
                throw new Exception("Invalid API key supplied");
            


            //read more on claim
            //https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-6.0
            /**
             * A claims-based identity is the set of claims. A claim is a statement that an entity 
             * (a user or another application) makes about itself, it's just a claim. For example a 
             * claim list can have the user’s name, user’s e-mail, user’s age, user's authorization for an action
             */

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, auth.ApiKey),
                new Claim(ClaimTypes.Name, auth.UserName)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            throw new Exception("401 unauthorized");
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            throw new Exception("403 forbidden access");
        }
    }
}