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
    public class LotController : ApiController
    {
        private readonly LotService _lotService;
        private readonly StockExchangeService _exchangeService;

        public LotController(LotService lotService, StockExchangeService exchangeService)
        {
            _lotService = lotService;
            _exchangeService = exchangeService;
        }


        public IEnumerable<Lot> Get(bool isSell)
        {
            return _lotService.GetAllLots(isSell);
        }


        public Lot Get(int id)
        {
            return _lotService.GetLot(id);
        }


        public void Post([FromBody]Lot value)
        {
            
        }


    }
}
