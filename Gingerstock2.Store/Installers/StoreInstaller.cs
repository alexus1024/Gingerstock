using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Gingerstock2.Store.Installers
{

    public class StoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDbService>().ImplementedBy<SqlLiteDbService>().LifestyleSingleton());
        }
    }
}