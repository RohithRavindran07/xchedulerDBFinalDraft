using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(xchedulerDBFinalDraft.Startup))]
namespace xchedulerDBFinalDraft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
