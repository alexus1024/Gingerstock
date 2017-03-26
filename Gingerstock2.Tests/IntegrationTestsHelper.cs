using System;
using Gingerstock2.Store;

namespace Gingerstock2.Tests
{
    static class IntegrationTestsHelper
    {
        public static void ClearDbFast(IDbService dbSrv)
        {
            using (var db = dbSrv.GetDb())
            {
                var lots = db.Database.ExecuteSqlCommand("DELETE FROM Lots");
                Console.WriteLine($"Removed {lots} Lots");
                var trs = db.Database.ExecuteSqlCommand("DELETE FROM Transactions");
                Console.WriteLine($"Removed {trs} Transactions");
            }
        }

        public static TestStepData Buy(int count, Decimal price)
        {
            return new TestStepData(false, count, price);
        }
        public static TestStepData Sell(int count, Decimal price)
        {
            return new TestStepData(true, count, price);
        }

    }
}