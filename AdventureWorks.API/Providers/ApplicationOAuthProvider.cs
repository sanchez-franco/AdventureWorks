using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AdventureWorks.Business.Authentication;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Unity;

namespace AdventureWorks.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly IAuthenticationService _authenticationService;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            _publicClientId = publicClientId;

            _authenticationService = UnityConfig.Container.Resolve<IAuthenticationService>();
        }

        //https://msdn.microsoft.com/en-us/library/microsoft.owin.security.oauth.oauthauthorizationserverprovider.grantresourceownercredentials(v=vs.113).aspx
        //Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has 
        //provided name and password credentials directly into the client application's user interface, and the client application
        //is using those to acquire an "access_token" and optional "refresh_token". If the web application supports the resource owner
        //credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an access token 
        //the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        //with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        //The default behavior is to reject this grant type. See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //testing for rest client access
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var user = await Task.Factory.StartNew(() =>
                _authenticationService.ValidateUserPassword(context.UserName, context.Password)
            );

            context.Response.Headers.Append("IsValid", (user != null).ToString());

            if (user == null)
            {
                context.Rejected();
                return;
            }

            context.Response.Headers.Append("User", JsonConvert.SerializeObject(user));

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));

            var props = CreateProperties(context.UserName);

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "UserName", userName }
            };

            return new AuthenticationProperties(data);
        }
    }
}