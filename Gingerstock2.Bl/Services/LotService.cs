using System;
using System.Collections.Generic;
using System.Linq;
using Gingerstock2.Store;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Bl.Services
{
    public class LotService: BlServiceBase
    {
        public LotService(IDbService db):base(db)
        {
        }

        public List<Lot> GetOpenedLots(bool? isSell)
        {
            using (var db = Db.GetDb())
            {
                IEnumerable<Lot> query = db.Lots;
                if (isSell != null && isSell == true)
                {
                    query = query.Where(x => x.Quantity > 0)
                        .Where(x => x.ClosedQuantity < x.Quantity);
                }

                if (isSell != null && isSell == false)
                {
                    query = query.Where(x => x.Quantity < 0)
                        .Where(x => x.ClosedQuantity > x.Quantity);
                }

                query = query.OrderByDescending(x => x.StartTime);

                // скроем от следущих уровней тонкость про отрицательные величины, им это не к чему
                var ret = query.ToList();
                ret.ForEach(x =>
                {
                    x.Quantity = Math.Abs(x.Quantity);
                    x.ClosedQuantity = Math.Abs(x.ClosedQuantity);
                });
                return ret;
            }
        }

        public Lot GetLot(int id)
        {
            using (var db = Db.GetDb())
            {
                return db.GetById<Lot>(id);
            }
        }
    }
}