using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gingerstock2.Store.Models;
using Gingerstock2.Store.Services;

namespace Gingerstock2.Bl.Services
{
    /// <summary>
    ///     Сервис, обеспечивающий логику работы биржы - регистрацию лотов, поиск и исполнение сделок.
    /// </summary>
    public class StockExchangeService : BlServiceBase
    {
        public StockExchangeService(IGingerstockDbService db) : base(db)
        {
        }

        public int NewSellLot(decimal price, int quantity, string email)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            return NewLotHelper(price, quantity, email);
        }

        public int NewBuyLot(decimal price, int quantity, string email)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            return NewLotHelper(price, -quantity, email);
        }

        public void DoDeals(int lotId)
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

                var countToDealTotal = lot.AbsoluteRestQuantity();


                // найти подходящие противоположные лоты
                var oppositeLots = db.Lots
                    .WhereOpposite(lot.Quantity)
                    .WhereNotClosed()
                    .WherePriceFits(lot.Price, lot.Quantity)
                    .OrderByProfitability(lot.Quantity)
                    .ToList();

                // подготовить и расчитать сделки
                var dealQuantityRest = countToDealTotal;
                var deals = new List<DealPrepareInternalModel>();
                foreach (var oppositeLot in oppositeLots)
                {
                    var sellLot = lot.IsSell() ? lot : oppositeLot;
                    var buyLot = lot.IsSell() ? oppositeLot : lot;

                    var countToDeal = Math.Min(oppositeLot.AbsoluteRestQuantity(), dealQuantityRest);
                    var newDeal = new DealPrepareInternalModel
                    {
                        Count = countToDeal,
                        BuyLot = buyLot,
                        SellLot = sellLot,
                        Price = oppositeLot.Price
                    };
                    deals.Add(newDeal);

                    dealQuantityRest -= countToDeal;
                    Debug.Assert(dealQuantityRest >= 0);
                    if (dealQuantityRest == 0)
                        break;
                }

                Debug.Assert(deals.Sum(x => x.Count) <= countToDealTotal);

                // зарегистировать сделки
                // (тут по идее надо пологаться на оптимистичную блокировку и обрабатывать её ошибки, но оставлю это вне рамок данного проекта)    

                foreach (var deal in deals)
                {
                    var newTransaction = new Transaction
                    {
                        Price = deal.Price,
                        Quantity = deal.Count,
                        BuyLot = deal.BuyLot,
                        SellLot = deal.SellLot,
                        BuyBorkerEmail = deal.BuyLot.BrokerEmail,
                        SellBorkerEmail = deal.SellLot.BrokerEmail,
                        BuyLotTime = deal.BuyLot.StartTime,
                        SellLotTime = deal.SellLot.StartTime,
                        Time = time
                    };
                    db.Transactions.Add(newTransaction);

                    IncreaceClosedQuantityHelper(deal.BuyLot, deal.Count);
                    IncreaceClosedQuantityHelper(deal.SellLot, deal.Count);

                    Debug.Assert(Math.Abs(deal.BuyLot.ClosedQuantity) <= Math.Abs(deal.BuyLot.Quantity));
                    Debug.Assert(Math.Abs(deal.SellLot.ClosedQuantity) <= Math.Abs(deal.SellLot.Quantity));
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        ///     Добавляет count к ClosedQuantity по модулю (без учёта знака)
        /// </summary>
        private void IncreaceClosedQuantityHelper(Lot lot, int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (lot.Quantity == 0) throw new ArgumentException("lot with zero Quantity", nameof(lot));

            lot.ClosedQuantity = Math.Sign(lot.Quantity)*(Math.Abs(lot.ClosedQuantity) + count);
        }

        private int NewLotHelper(decimal price, int quantity, string email)
        {
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));

            int lotId;
            using (var db = Db.GetDb())
            {
                var lot = new Lot
                {
                    Price = price,
                    BrokerEmail = email,
                    Quantity = quantity,
                    StartTime = DateTime.Now
                };
                db.Lots.Add(lot);
                db.SaveChanges();
                lotId = lot.Id;
            }

            DoDeals(lotId);
            return lotId;
        }

        private class DealPrepareInternalModel
        {
            public int Count { get; set; }
            public Lot BuyLot { get; set; }
            public Lot SellLot { get; set; }
            public decimal Price { get; set; }
        }
    }
}