using Gingerstock2.Store;

namespace Gingerstock2.Bl.Services
{
    public abstract class BlServiceBase
    {
        protected IDbService Db { get; private set; }

        protected BlServiceBase(IDbService db)
        {
            Db = db;
        }
    }
}