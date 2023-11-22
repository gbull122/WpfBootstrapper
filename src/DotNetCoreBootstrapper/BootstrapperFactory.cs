[assembly: WixToolset.Mba.Core.BootstrapperApplicationFactory(typeof(DotNetCoreBootstrapper.BootstrapperFactory))]
namespace DotNetCoreBootstrapper;

using WixToolset.Mba.Core;

public class BootstrapperFactory : BaseBootstrapperApplicationFactory
{
    private static int loadCount = 0;

    protected override IBootstrapperApplication Create(IEngine engine, IBootstrapperCommand bootstrapperCommand)
    {
        if (loadCount > 0)
        {
            engine.Log(LogLevel.Standard, $"Reloaded {loadCount} time(s)");
        }
        ++loadCount;
        return new Bootstrapper(engine, bootstrapperCommand);
    }
}
