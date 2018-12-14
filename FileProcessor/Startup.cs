using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileProcessor.Startup))]
namespace FileProcessor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
