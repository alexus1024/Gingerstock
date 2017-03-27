namespace Gingerstock2.Store.Services
{
    public interface IGingerstockDbService
    {
        IGingerstock2Context GetDb();
    }


    class SqlLiteGingerstockDbService : IGingerstockDbService
    {
        public IGingerstock2Context GetDb()
        {
            return new Gingerstock2Context();
        }
    }
}