namespace Rsdn.Client.Data.Storage
{
    using SQLite;

    public interface IDatabaseFactory
    {
        SQLiteConnection GetDatabase();
    }
}