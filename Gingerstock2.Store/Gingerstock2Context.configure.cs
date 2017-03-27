using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;

namespace Gingerstock2.Store
{
    public partial class Gingerstock2Context : DbContext, IGingerstock2Context
    {
        static Gingerstock2Context()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Gingerstock2Context, ContextMigrationConfiguration>(true));
        }

        public Gingerstock2Context() : base("Gingerstock2Db")
        {
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
    }
}