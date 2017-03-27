using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gingerstock2.Bl.Services;
using Gingerstock2.Store.Models;

namespace Gingerstock.Web.Controllers
{
    public abstract class LotController : ApiController
    {
        private readonly bool _isSell;
        private readonly LotService _lotService;
        private readonly StockExchangeService _exchangeService;

        protected LotController(Boolean isSell, LotService lotService, StockExchangeService exchangeService)
        {
            _isSell = isSell;
            _lotService = lotService;
            _exchangeService = exchangeService;
        }


        public IEnumerable<Lot> Get()
        {
            return  _lotService.GetOpenedLots(_isSell);
        }


        public Lot Get(int id)
        {
            return _lotService.GetLot(id);
        }


        public void Post([FromBody]Lot value)
        {
            if (_isSell)
                _exchangeService.NewSellLot(value.Price, value.Quantity, value.BrokerEmail);
            else
                _exchangeService.NewBuyLot(value.Price, value.Quantity, value.BrokerEmail);
        }


    }

    public class BuyLotController : LotController
    {
        public BuyLotController(LotService lotService, StockExchangeService exchangeService) : base(false, lotService, exchangeService)
        {
        }
    }

    public class SellLotController : LotController
    {
        public SellLotController(LotService lotService, StockExchangeService exchangeService) : base(true, lotService, exchangeService)
        {
        }
    }
}
