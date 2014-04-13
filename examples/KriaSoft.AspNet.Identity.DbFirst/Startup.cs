using KriaSoft.AspNet.Identity.DbFirst;
using KriaSoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace KriaSoft.AspNet.Identity.DbFirst
{
    using System.Data.Entity;
    using System.Linq;
    using System.Text;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var db = new ApplicationDbContext();
            var userManager = new UserManager<User, int>(new UserStore(db));
            var roleManager = new RoleManager<UserRole, int>(new RoleStore(db));

            var sb = new StringBuilder();

            var format = "{0, -3} | {1, -15} | {2, -30} | {3, -20}";
            sb.AppendLine("ASP.NET Identity Users:\n");
            sb.AppendLine(string.Format(format, "ID", "Username", "Email", "Role(s)"));
            sb.AppendLine("-----------------------------------------------------------------------------");

            foreach (var user in userManager.Users.Include(u => u.Roles))
            {
                sb.AppendLine(string.Format(format, user.Id, user.UserName, user.Email, string.Join(", ", user.Roles.Select(r => r.Name).ToArray())));
            }

            sb.AppendLine("\n\nASP.NET Identity Roles:\n");
            format = "{0, -3} | {1, -20}";
            sb.AppendLine(string.Format(format, "ID", "Role"));
            sb.AppendLine("--------------------------");

            foreach (var role in roleManager.Roles)
            {
                sb.AppendLine(string.Format(format, role.Id, role.Name));
            }

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync(sb.ToString());
            });
        }
    }
}
