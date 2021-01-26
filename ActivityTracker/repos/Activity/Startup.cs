using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Activity.Startup))]
namespace Activity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
