using KriaSoft.AspNet.Identity.DbFirst;
using KriaSoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KriaSoft.AspNet.Identity.DbFirst
{
    using System.Linq;
    using System.Text;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var db = new ApplicationDbContext();
            var userManager = new UserManager<User, int>(new UserStore(db));

            var sb = new StringBuilder();
            var format = "{0, -3} | {1, -15} | {2, -30}";
            sb.AppendLine(string.Format(format, "ID", "Username", "Email"));
            sb.AppendLine("------------------------------------------------------");

            foreach (var user in userManager.Users)
            {
                sb.AppendLine(string.Format(format, user.Id, user.UserName, user.Email));
            }

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("ASP.NET Identity Users: \n\n" + sb.ToString());
            });
        }
    }
}
