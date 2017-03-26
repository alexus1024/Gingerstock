using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gingerstock2.Store;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Bl
{
    public class StockExchange
    {
        protected IDbService Db { get; private set; }

        public StockExchange(IDbService db)
        {
            Db = db;
        }

        public void NewSellLot(Decimal price, Int32 quantity, String email)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            NewLot(price, quantity, email);
        }

        public void NewBuyLot(Decimal price, Int32 quantity, String email)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            NewLot(price, -quantity, email);
        }

        public void DoDeals(Int32 lotId)
        {
            var time = DateTime.Now;

            using (var db = Db.GetDb())
            {
                // получить данные о лоте
                var lot = db.GetById<Lot>(lotId);
                if (lot.IsClosed())
                {
                    throw new BlException($"Лот {lotId} уже закрыт");
                }

                var countToDealTotal = lot.Quantity - lot.ClosedQuantity;


                // найти подходящие противоположные лоты
                var oppositeLots = db.Lots
                    .WhereOpposite(lot.Quantity)
                    .WhereNotClosed()
                    .WherePriceFits(lot.Price, lot.Quantity)
                    .OrderByProfitability(lot.Quantity)
                    .ToList();
                    

                // подготовить сделки
                var oppositeQuantityAccum = 0;
                var oppositeLotsByCount = oppositeLots.TakeWhile( // TO TEST
                    opposite => (oppositeQuantityAccum += opposite.Quantity - opposite.ClosedQuantity) <= countToDealTotal)
                    .ToList();

                var dealQuantityRest = countToDealTotal;
                var deals = new List<DealPrepareInternalModel>();
                foreach (var oppositeLot in oppositeLots)
                {
                    var sellLot = lot.IsSell()? lot:oppositeLot;
                    var buyLot = lot.IsSell() ? oppositeLot : lot;

                    var countToDeal = Math.Min(oppositeLot.Quantity - oppositeLot.ClosedQuantity, dealQuantityRest);
                    var newDeal = new DealPrepareInternalModel()
                    {
                        Count = countToDeal,
                        BuyLot = buyLot,
                        SellLot = sellLot,
                        Price = oppositeLot.Price,
                    };
                    deals.Add(newDeal);

                    dealQuantityRest -= countToDeal;
                    Debug.Assert(dealQuantityRest >= 0);
                    if (dealQuantityRest == 0) 
                        break;
                    
                }

                Debug.Assert(deals.Sum(x => x.Count) == countToDealTotal);

                // зарегистировать сделки
                // (тут по идее надо пологаться на оптимистичную блокировку и обрабатывать её ошибки, но оставлю это вне рамок данного проекта)    

                foreach (var deal in deals)
                {
                    var newTransaction = new Transaction()
                    {
                        Price = deal.Price,
                        Count = deal.Count,
                        BuyLot = deal.BuyLot,
                        SellLot = deal.SellLot,
                        BuyBorkerEmail = deal.BuyLot.BrokerEmail,
                        SellBorkerEmail = deal.SellLot.BrokerEmail,
                        BuyLotTime = deal.BuyLot.StartTime,
                        SellLotTime = deal.SellLot.StartTime,
                        Time = time,

                    };
                    db.Transactions.Add(newTransaction);

                    deal.BuyLot.ClosedQuantity = IncreaceClosedQuantityHelper(deal.BuyLot.ClosedQuantity, deal.Count);
                    deal.SellLot.ClosedQuantity = IncreaceClosedQuantityHelper(deal.SellLot.ClosedQuantity, deal.Count);

                    Debug.Assert(deal.BuyLot.ClosedQuantity <= deal.BuyLot.Quantity);
                    Debug.Assert(deal.SellLot.ClosedQuantity <= deal.SellLot.Quantity);
                }

                db.SaveChanges();

            }
        }

        /// <summary>
        /// Добавляет ClosedQuantity по модулю (без учёта знака)
        /// </summary>
        private int IncreaceClosedQuantityHelper(int quantity, int count)
        {
            return Math.Sign(quantity) * (Math.Abs(quantity) + count);
        }

        class DealPrepareInternalModel
        {
            public int Count { get; set; }
            public Lot BuyLot { get; set; }
            public Lot SellLot { get; set; }
            public decimal Price { get; set; }
        }

        void NewLot(Decimal price, Int32 quantity, String email)
        {
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));

            Int32 lotId;
            using (var db = Db.GetDb())
            {
                var lot = new Lot()
                {
                    Price = price,
                    BrokerEmail = email,
                    Quantity = quantity,
                };
                db.Lots.Add(lot);
                db.SaveChanges();
                lotId = lot.Id;
            }

            DoDeals(lotId);
        }
    }
}