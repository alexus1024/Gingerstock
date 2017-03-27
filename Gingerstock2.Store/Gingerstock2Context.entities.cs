using System.Data.Entity;
using System.Linq;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Store
{
    public partial class Gingerstock2Context
    {
        public IDbSet<Lot> Lots { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        public TEntity GetById<TEntity>(int id) where TEntity : Gingerstock2EntityBase
        {
            return Set<TEntity>().Single(x => x.Id == id);
        }
    }
}