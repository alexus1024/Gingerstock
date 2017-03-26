namespace Gingerstock2.Store
{
    public interface IDbService
    {
        IGingerstock2Context GetDb();
    }


    class SqlLiteDbService : IDbService
    {
        public IGingerstock2Context GetDb()
        {
            return new Gingerstock2Context();
        }
    }
}