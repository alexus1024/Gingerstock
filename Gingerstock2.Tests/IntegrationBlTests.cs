using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Gingerstock2.Bl;
using Gingerstock2.Bl.Services;
using Gingerstock2.Store;
using NUnit.Framework;
using static Gingerstock2.Tests.IntegrationTestsHelper;

namespace Gingerstock2.Tests
{
    [TestFixture]
    public class IntegrationBlTests
    {
        public IWindsorContainer Container { get; set; }
        public IDbService Db { get; set; }

        [OneTimeSetUp]
        public  void SetUpFixture()
        {
            WindsorContainer c = new WindsorContainer();
            c.Install(FromAssembly.InThisApplication());
            Container = c;
            Db = c.Resolve<IDbService>();
        }


        [SetUp]
        public void SetUp()
        {
            ClearDbFast(Db);
        }


        public static IEnumerable<TestCaseData> TestDealCases
        {
            get
            {
                yield return new TestCaseData(
                    new[] {Buy(1, 1), Sell(1, 2),})
                    .Returns(new TestDealResult(0, 0, 0))
                    .SetName("Buy sell no deal");
                yield return new TestCaseData(
                    new[] {Sell(1, 2), Buy(1, 1),})
                    .Returns(new TestDealResult(0, 0, 0))
                    .SetName("Sell buy sell no deal");

                yield return new TestCaseData(
                    new[] { Buy(1, 2), Buy(1, 3), Sell(2, 1), })
                    .Returns(new TestDealResult(2, 2, 5))
                    .SetName("Buy sell 2 deal finished");
                yield return new TestCaseData(
                    new[] {Buy(2, 2), Buy(2, 3), Sell(3, 1),})
                    .Returns(new TestDealResult(2, 3, 8)) // 2x3 + 1x2
                    .SetName("Buy sell 2 deal ufinished 1");
                yield return new TestCaseData(
                    new[] {Sell(1, 1), Sell(1, 2), Sell(1, 3), Sell(1, 4), Buy(3, 5),})
                    .Returns(new TestDealResult(3, 3, 6)) // 1+2+3
                    .SetName("Sell buy sell unfinished 1");

                yield return new TestCaseData(
                    new[] {Sell(10, 1), Sell(10, 2), Buy(15, 5), Sell(10, 3), Buy(20, 5)})
                    .Returns(new TestDealResult(4, 30, 20+40)) // (10*1+5*2) + (5*2 + 10*3)
                    .SetName("Sepatrate deals 1");
            }
        }


        [Test, TestCaseSource(nameof(TestDealCases))]
        public TestDealResult TestDeal(TestStepData[] steps)
        {
            var exchange = Container.Resolve<StockExchangeService>();

            foreach (var step in steps)
            {
                if (step.IsSell)
                    exchange.NewSellLot(step.Price, step.Count, null);
                else
                    exchange.NewBuyLot(step.Price, step.Count, null);
            }

            using (var db = Db.GetDb())
            {
                var lots = db.Lots.ToList();
                var trs = db.Transactions.ToList();

                var sellSteps = steps.Count(x => x.IsSell);
                Assert.AreEqual(sellSteps, lots.Count(x => x.IsSell()));
                var buySteps = steps.Count(x => !x.IsSell);
                Assert.AreEqual(buySteps, lots.Count(x => !x.IsSell()));

                return new TestDealResult
                {
                    TrTotalCount = trs.Count,
                    TrTotalQuantity = trs.Sum(x => x.Quantity),
                    TrTotalMoney = trs.Sum(x => x.Quantity * x.Price),
                };
            }
        }
        
    }

}