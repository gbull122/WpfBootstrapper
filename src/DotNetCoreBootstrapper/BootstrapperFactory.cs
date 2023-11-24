[assembly: WixToolset.Mba.Core.BootstrapperApplicationFactory(typeof(DotNetCoreBootstrapper.BootstrapperFactory))]
namespace DotNetCoreBootstrapper;

using WixToolset.Mba.Core;

public class BootstrapperFactory : BaseBootstrapperApplicationFactory
{

    protected override IBootstrapperApplication Create(IEngine engine, IBootstrapperCommand bootstrapperCommand)
    {
        return new Bootstrapper(engine, bootstrapperCommand);
    }
}
