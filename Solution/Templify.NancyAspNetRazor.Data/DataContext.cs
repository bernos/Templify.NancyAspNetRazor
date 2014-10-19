using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Templify.NancyAspNetRazor.Data.Auth.Models;

namespace Templify.NancyAspNetRazor.Data
{
    public class DataContext : DbContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<Claim> Claims { get; set; }

        public DataContext()
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Configurations.AddFromAssembly(typeof (DataContext).Assembly);
        }
    }
}
