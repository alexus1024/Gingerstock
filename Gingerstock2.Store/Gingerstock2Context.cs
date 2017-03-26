using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Migrations;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.EF6.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Gingerstock2.Store.Models;

namespace Gingerstock2.Store
{

    //public class SQLiteConfiguration : DbConfiguration
    //{
    //    public SQLiteConfiguration()
    //    {
    //        SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
    //        SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
    //        SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
    //    }
    //}

    //[DbConfigurationType(typeof(SQLiteConfiguration))]
    public class Gingerstock2Context : DbContext, IGingerstock2Context
    {
        static Gingerstock2Context()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Gingerstock2Context, ContextMigrationConfiguration>(true));
        }

        internal sealed class ContextMigrationConfiguration : DbMigrationsConfiguration<Gingerstock2Context>
        {
            public ContextMigrationConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
                SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
            }

            protected override void Seed(Gingerstock2Context context)
            {
                base.Seed(context);
            }
        }

        public Gingerstock2Context() : base("Gingerstock2Db")
        {
        }

        public IDbSet<Lot> Lots { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        public TEntity GetById<TEntity>(Int32 id) where TEntity : Gingerstock2Entity
        {
            return Set<TEntity>().Single(x => x.Id == id);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }


}
