using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gingerstock2.Store.Models
{
    public abstract class Gingerstock2Entity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
    }

    public class Lot: Gingerstock2Entity
    {
        public Decimal Price { get; set; }

        /// <summary>
        /// Положителное число - продажа, отрицательное - покупка
        /// </summary>
        public Int32 Quantity { get; set; }

        /// <summary>
        /// Количество, которое было продано.
        /// Если Quantity == ClosedQuantity - лот закрыт
        /// </summary>
        public Int32 ClosedQuantity { get; set; }
        public String BrokerEmail { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class Transaction : Gingerstock2Entity
    {
        public DateTime Time { get; set; }
        public DateTime SellLotTime  { get; set; }
        public DateTime BuyLotTime { get; set; }
        public Decimal Price { get; set; }
        public Int32 Count { get; set; }
        public String SellBorkerEmail { get; set; }
        public String BuyBorkerEmail { get; set; }

        public int? SellLotId { get; set; }
        public Lot SellLot { get; set; }
        public int? BuyLotId { get; set; }
        public Lot BuyLot { get; set; }
    }


}