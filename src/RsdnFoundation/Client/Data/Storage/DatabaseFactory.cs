namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;
    using Windows.Storage;

    public class DatabaseFactory : IDatabaseFactory
    {
        private static readonly string databasePath;

        static DatabaseFactory()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            databasePath = Path.Combine(localFolder.Path, "rsdn.dat");
        }

        public static void CreateDatabase()
        {
            if (File.Exists(databasePath))
                return;

            using (var connection = GetDatabaseStatic())
            {
                connection.CreateTable<DbGroup>();
                connection.CreateTable<DbForum>();
                connection.CreateTable<DbPost>();
                connection.CreateTable<DbThread>();
                connection.CreateTable<DbReply>();
                connection.CreateTable<DbRating>();
                connection.CreateTable<DbUser>();
            }
        }

        public SQLiteConnection GetDatabase()
        {
            return GetDatabaseStatic();
        }

        private static SQLiteConnection GetDatabaseStatic()
        {
            var connection = new SQLiteConnection(databasePath,
                openFlags: SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex)
            {
#if DEBUG
                Trace = true
#endif
            };
            return connection;
        }
    }
}