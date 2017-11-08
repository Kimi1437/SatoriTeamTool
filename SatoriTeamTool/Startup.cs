using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SatoriTeamTool.Startup))]
namespace SatoriTeamTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
