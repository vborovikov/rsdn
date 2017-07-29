namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Threading;
    using Microsoft.EntityFrameworkCore.Storage;

    internal class TransactionWrapper : IDisposable
    {
        private readonly CancellationToken cancelToken;
        private readonly IDbContextTransaction transaction;

        public TransactionWrapper(CancellationToken cancelToken)
        {
            this.cancelToken = cancelToken;
            this.Connection = new RsdnDbContext();
            this.transaction = this.Connection.Database.BeginTransaction();

            cancelToken.Register(Cancel);
        }

        public RsdnDbContext Connection { get; }

        public void Dispose()
        {
            if (this.cancelToken.IsCancellationRequested == false)
            {
                this.Connection.SaveChanges();
                this.transaction.Commit();
            }
            this.Connection.Dispose();
        }

        private void Cancel()
        {
            this.transaction.Rollback();
        }
    }
}