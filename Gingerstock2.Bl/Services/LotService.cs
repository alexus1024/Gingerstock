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

        public List<Lot> GetAllLots(bool? isSell)
        {
            using (var db = Db.GetDb())
            {
                IEnumerable<Lot> query = db.Lots;
                if (isSell != null && isSell == true)
                {
                    query = query.Where(x => x.Quantity > 0);
                }

                if (isSell != null && isSell == false)
                {
                    query = query.Where(x => x.Quantity < 0);
                }

                return query.ToList();
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