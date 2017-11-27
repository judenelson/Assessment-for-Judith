using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeedingAdminUsers.Startup))]
namespace SeedingAdminUsers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
