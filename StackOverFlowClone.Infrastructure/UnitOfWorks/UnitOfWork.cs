using NHibernate;

namespace StackOverFlowClone.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        protected ISession _session;
        private ITransaction _transaction = null!;

        public UnitOfWork(ISession session)
        {
            _session = session;
        }

        public void SaveChanges()
        {
            _session.Flush();
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            CloseTransaction();
        }

        public void RollBackTransaction()
        {
            _transaction.Rollback();

            CloseTransaction();
            CloseSession();
        }

        private void CloseTransaction()
        {
            _transaction.Dispose();
            _transaction = null!;
        }

        private void CloseSession()
        {
            _session.Close();
            _session.Dispose();
            _session = null!;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                CommitTransaction();
            }

            if (_session != null)
            {
                SaveChanges();
                CloseSession();
            }
        }
    }
}
