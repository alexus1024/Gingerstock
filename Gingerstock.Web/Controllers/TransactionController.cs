using System.Collections.Generic;
using System.Web.Http;
using Gingerstock2.Bl.Services;
using Gingerstock2.Store.Models;

namespace Gingerstock.Web.Controllers
{
    public class TransactionController : ApiController{
        private readonly TransactionService _transactionService;


        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        public IEnumerable<Transaction> Get()
        {
            return _transactionService.GetLastTransactions();
        }

    }
}