using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Templify.NancyAspNetRazor.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        int Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _context;
        private bool _disposed;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository { get { return new UserRepository(_context); } }

        public int Commit()
        {
            return _context.SaveChanges();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
