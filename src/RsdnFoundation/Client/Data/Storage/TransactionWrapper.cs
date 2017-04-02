namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using SQLite;

    internal class TransactionWrapper : IDisposable
    {
        private readonly CancellationToken cancelToken;

        public TransactionWrapper(IDatabaseFactory databaseFactory, CancellationToken cancelToken)
        {
            this.cancelToken = cancelToken;
            this.Connection = databaseFactory.GetDatabase();
            this.Connection.BeginTransaction();

            cancelToken.Register(Cancel);
        }

        public SQLiteConnection Connection { get; }

        public void Dispose()
        {
            if (this.cancelToken.IsCancellationRequested == false)
            {
                this.Connection.Commit();
            }
            this.Connection.Dispose();
        }

        private void Cancel()
        {
            this.Connection.Rollback();
        }
    }
}