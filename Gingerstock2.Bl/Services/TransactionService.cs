using System.Collections.Generic;
using System.Linq;
using Gingerstock2.Store;
using Gingerstock2.Store.Models;
using Gingerstock2.Store.Services;

namespace Gingerstock2.Bl.Services
{
    public class TransactionService : BlServiceBase
    {
        public TransactionService(IGingerstockDbService db) : base(db)
        {
        }

        public List<Transaction> GetLastTransactions()
        {
            using (var db = Db.GetDb())
            {
                return db.Transactions.OrderByDescending(x => x.Time).Take(100).ToList();
            }
        }
    }
}