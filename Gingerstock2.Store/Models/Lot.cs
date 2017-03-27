using System;

namespace Gingerstock2.Store.Models
{
    public class Lot: Gingerstock2EntityBase
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
}