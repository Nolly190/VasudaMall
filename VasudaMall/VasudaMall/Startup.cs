using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VasudaMall.Startup))]
namespace VasudaMall
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
