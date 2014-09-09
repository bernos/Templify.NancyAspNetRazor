using System.Collections.ObjectModel;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var administratorClaim = new Claim
            {
                Name = Claim.IsAdministrator
            };

            context.Claims.AddOrUpdate(c => c.Name, administratorClaim);
            context.SaveChanges();
            
            var adminRole = new Role
            {
                Name = Role.Administrator,
                Claims = new Collection<Claim>
                {
                    administratorClaim
                }
            };

            context.Roles.AddOrUpdate(r => r.Name, adminRole);
            context.SaveChanges();

            context.Users.AddOrUpdate(u => u.UserName, new User
            {
                UserName = "admin",
                Roles = new Collection<Role> { adminRole }
            });

            context.SaveChanges();
        }
    }
}
