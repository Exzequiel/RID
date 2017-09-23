using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RID.Startup))]
namespace RID
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
