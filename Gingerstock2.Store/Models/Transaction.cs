using System;

namespace Gingerstock2.Store.Models
{
    public class Transaction : Gingerstock2EntityBase
    {
        public DateTime Time { get; set; }
        public DateTime SellLotTime  { get; set; }
        public DateTime BuyLotTime { get; set; }
        public Decimal Price { get; set; }
        public Int32 Quantity { get; set; }
        public String SellBorkerEmail { get; set; }
        public String BuyBorkerEmail { get; set; }

        public int? SellLotId { get; set; }
        public Lot SellLot { get; set; }
        public int? BuyLotId { get; set; }
        public Lot BuyLot { get; set; }
    }
}