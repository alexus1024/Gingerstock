using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Gingerstock2.Bl;
using Gingerstock2.Store;
using NUnit.Framework;

namespace Gingerstock2.Tests
{

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void PeekDb()
        {
            using (var db =new Gingerstock2Context())
            {
                var lots = db.Lots.ToList();
            }
        }
        [Test]
        public void SimpleDeal()
        {

            WindsorContainer c = new WindsorContainer();
            c.Install(FromAssembly.InThisApplication());

            var exchange = c.Resolve<StockExchange>();

            exchange.NewSellLot(10, 1, null);
            exchange.NewBuyLot(10, 1, null);
        }


    }
}
