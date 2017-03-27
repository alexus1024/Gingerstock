using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Gingerstock2.Bl.Services;

namespace Gingerstock2.Bl.Installers
{
    public class BlServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // лучше дополнительно использовать интерфейсы сервисов для регистрации их в IoC, но в рамках данного проекта заниматься этим не будем.
            container.Register(Component.For<StockExchangeService>().LifestyleTransient());
            container.Register(Component.For<LotService>().LifestyleTransient());
            container.Register(Component.For<TransactionService>().LifestyleTransient());
        }
    }

    
}