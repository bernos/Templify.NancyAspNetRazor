using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Templify.NancyAspNetRazor.Data.Auth.Repositories;

namespace Templify.NancyAspNetRazor.Data
{
    public interface IUnitOfWork : Bernos.DDD.Data.IUnitOfWork, IDisposable
    {
        IUserRepository UserRepository { get; }
    }

    public class UnitOfWork : Bernos.DDD.Data.EntityFramework.UnitOfWork, IUnitOfWork
    {   
        public UnitOfWork(DbContext context) : base(context)
        {
        }

        public IUserRepository UserRepository { get { return new UserRepository(_context); } }
    }
}
