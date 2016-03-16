using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Example.Startup))]
namespace Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
