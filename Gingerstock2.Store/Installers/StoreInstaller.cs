using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Gingerstock2.Store.Services;

namespace Gingerstock2.Store.Installers
{

    public class StoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IGingerstockDbService>().ImplementedBy<SqlLiteGingerstockDbService>().LifestyleSingleton());
        }
    }
}