using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using KriaSoft.AspNet.Identity.EntityFramework;

namespace KriaSoft.AspNet.Identity.DbFirst.Security
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string publicClientId;
        private readonly Func<UserManager<User, int>> userManagerFactory;

        public ApplicationOAuthProvider(string publicClientId, Func<UserManager<User, int>> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            this.publicClientId = publicClientId;
            this.userManagerFactory = userManagerFactory;
        }

        public static AuthenticationProperties CreateProperties(User user)
        {
            return new AuthenticationProperties(new Dictionary<string, string>
            {
                { "userName", user.UserName },
                { "email", user.Email },
                { "emailConfirmed", user.EmailConfirmed.ToString() }
            });
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var userManager = this.userManagerFactory())
            {
                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var oauthIdentity = await userManager.CreateIdentityAsync(
                    user, context.Options.AuthenticationType);
                var cookiesIdentity = await userManager.CreateIdentityAsync(
                    user, CookieAuthenticationDefaults.AuthenticationType);
                var properties = CreateProperties(user);
                var ticket = new AuthenticationTicket(oauthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
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
            if (context.ClientId == this.publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }
    }
}