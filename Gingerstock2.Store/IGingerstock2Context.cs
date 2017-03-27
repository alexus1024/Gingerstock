using System;
using System.Data.Entity;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Store
{
    public interface IGingerstock2Context : IDisposable
    {
        IDbSet<Lot> Lots { get; }
        IDbSet<Transaction> Transactions { get; }
        TEntity GetById<TEntity>(Int32 id) where TEntity : Gingerstock2EntityBase;
        int SaveChanges();

        Database Database { get; }
    }
}