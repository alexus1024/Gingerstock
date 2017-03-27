using System.Collections.Generic;
using System.Linq;
using Gingerstock2.Store;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Bl.Services
{
    public class TransactionService : BlServiceBase
    {
        public TransactionService(IDbService db) : base(db)
        {
        }

        public List<Transaction> GetAllTransactions()
        {
            using (var db = Db.GetDb())
            {
                return db.Transactions.ToList();
            }
        }
    }
}