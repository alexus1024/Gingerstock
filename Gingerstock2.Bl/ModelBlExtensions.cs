using System;
using System.Linq;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Bl
{
    public static class ModelBlExtensions
    {
        public static bool IsClosed(this Lot that)
        {
            return that.ClosedQuantity == that.Quantity;
        }

        public static bool IsSell(this Lot that)
        {
            return that.Quantity>0;
        }

        public static IQueryable<Lot> WhereNotClosed(this IQueryable<Lot> that)
        {
            return that.Where(x => x.ClosedQuantity != x.Quantity);
        }

        public static IQueryable<Lot> WhereOpposite(this IQueryable<Lot> that, int quantity)
        {
            var sign = Math.Sign(quantity);
            return that.Where(x => Math.Sign(quantity)!= sign);
        }

        /// <summary>
        /// Фильтрует предложения по соответсвию цены.
        /// Если лот продающий - то ему подойдёт покупающие лоты по более высокой цене;
        /// если покупающий - то по более низкой.
        /// </summary>
        public static IQueryable<Lot> WherePriceFits(this IQueryable<Lot> that, decimal price, int quantity)
        {
            var isSell = quantity > 0;

            if (isSell)
                return that.Where(opposite => opposite.Price >= price);
            else
                return that.Where(opposite => opposite.Price <= price);

        }

        /// <summary>
        /// Сортирует предложения по выгодности на единицу.
        /// если лот продающий - то самые выгодные покупающие - те, что с самой высокой ценой,
        /// и наоборот
        /// </summary>
        public static IOrderedQueryable<Lot> OrderByProfitability(this IQueryable<Lot> that, int quantity)
        {
            var isSell = quantity > 0;

            if (isSell)
                return that.OrderByDescending(opposite => opposite.Price);
            else
                return that.OrderBy(opposite => opposite.Price);

        }

    }
}