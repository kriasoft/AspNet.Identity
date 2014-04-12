using KriaSoft.AspNet.Identity.DbFirst;
using KriaSoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KriaSoft.AspNet.Identity.DbFirst
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var db = new ApplicationDbContext();
            var userManager = new UserManager<User, int>(new UserStore(db));
            var user = userManager.FindByNameAsync("demouser").Result;

            if (user == null)
            {
                var result = userManager.CreateAsync(new User { UserName = "demouser" }, "Passw0rd").Result;
                user = userManager.FindAsync("demouser", "Passw0rd").Result;
                System.Diagnostics.Debug.Assert(user != null && user.UserName == "demouser");
            }

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Entity Framework Database-First Provider for ASP.NET Identity");
            });
        }
    }
}
