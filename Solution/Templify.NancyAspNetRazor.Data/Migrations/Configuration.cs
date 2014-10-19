using System.Collections.ObjectModel;
using Bernos.Security;
using Templify.NancyAspNetRazor.Data.Auth.Models;

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

            var hasher = new Pbkdf2Sha1PasswordHasher(new Pbkdf2Sha1Configuration());

            var adminUser = new User("admin", hasher.CreateHash("admin"));
            adminUser.Roles.Add(adminRole);
            
            context.Users.AddOrUpdate(u => u.UserName, adminUser);

            context.SaveChanges();
        }
    }
}
