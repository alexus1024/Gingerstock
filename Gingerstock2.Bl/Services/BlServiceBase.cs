using Gingerstock2.Store;
using Gingerstock2.Store.Services;

namespace Gingerstock2.Bl.Services
{
    public abstract class BlServiceBase
    {
        protected IGingerstockDbService Db { get; private set; }

        protected BlServiceBase(IGingerstockDbService db)
        {
            Db = db;
        }
    }
}