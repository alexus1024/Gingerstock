//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Castle.Windsor;
//using Castle.Windsor.Installer;
//using Gingerstock2.Bl;
//using Gingerstock2.Store;
//using NUnit.Framework;

//namespace Gingerstock2.Tests
//{

//    [TestFixture]
//    public class IntegrationInfrastructureTests
//    {
//        [Test]
//        public void PeekDb()
//        {
//            using (var db = new Gingerstock2Context())
//            {
//                var lots = db.Lots.ToList();
//            }
//        }

//        [Test]
//        public void SimpleDeal()
//        {
//            WindsorContainer c = new WindsorContainer();
//            c.Install(FromAssembly.InThisApplication());

//            var dbSrv = c.Resolve<IDbService>();
//            IntegrationTestsHelper.ClearDbFast(dbSrv);
            

//            var exchange = c.Resolve<StockExchange>();

//            var sell = exchange.NewSellLot(10, 1, null);
//            var buy = exchange.NewBuyLot(10, 1, null);

//            using (var db = dbSrv.GetDb())
//            {
//                var tr = db.Transactions.Single();
//                Assert.AreEqual(sell, tr.SellLotId);
//                Assert.AreEqual(buy, tr.BuyLotId);
//                Assert.AreEqual(1, tr.Quantity);
//                Assert.AreEqual(10, tr.Price);

//            }
//        }




//    }

//}
