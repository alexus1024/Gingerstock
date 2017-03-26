using System;

namespace Gingerstock2.Tests
{

    public struct TestStepData
    {
        public TestStepData(bool isSell, int count, Decimal price)
        {
            IsSell = isSell;
            Count = count;
            Price = price;
        }

        public bool IsSell;
        public int Count;
        public Decimal Price;
    }

    public struct TestDealResult
    {
        public TestDealResult(int count, int quantity, Decimal money)
        {
            TrTotalCount = count;
            TrTotalQuantity = quantity;
            TrTotalMoney = money;
        }

        public override string ToString()
        {
            return $"TrCnt:{TrTotalCount} Qnt:{TrTotalQuantity} $:{TrTotalMoney}";
        }

        public int TrTotalCount;
        public int TrTotalQuantity;
        public Decimal TrTotalMoney;

    }
}