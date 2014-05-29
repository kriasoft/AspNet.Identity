using System;
using System.Web;

using KriaSoft.AspNet.Identity.DbFirst;
using KriaSoft.AspNet.Identity.DbFirst.Security;
using KriaSoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KriaSoft.AspNet.Identity.DbFirst
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Register UserManager in ApplicationDbContext in OWIN context
            app.CreatePerOwinContext<ApplicationDbContext>(() => new ApplicationDbContext());
            app.CreatePerOwinContext<UserManager<User, int>>(
                (IdentityFactoryOptions<UserManager<User, int>> options, IOwinContext context) =>
                    new UserManager<User, int>(new UserStore(context.Get<ApplicationDbContext>())));

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(
                    "self", () => HttpContext.Current.GetOwinContext().GetUserManager<UserManager<User, int>>()),
                AuthorizeEndpointPath = new PathString("/api/account/authorize"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            });
        }
    }
}
