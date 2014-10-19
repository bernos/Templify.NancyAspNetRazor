using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Bernos.DDD.Data.EntityFramework;
using Templify.NancyAspNetRazor.Data.Auth.Models;

namespace Templify.NancyAspNetRazor.Data.Auth.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context):base(context)
        {
        }

        public IEnumerable<string> GetClaimsForUser(Guid userId)
        {   
            var user = _dbSet
                        .Include(u => u.Claims)
                        .Include(u => u.Roles)
                        .SingleOrDefault(u => u.UserId == userId);

            var userClaims = user.Claims.Select(c => c.Name).ToList();
            var roleClaims = user.Roles.SelectMany(r => r.Claims.Select(c => c.Name)).ToList();
                
            return user == null ? new string[0] : userClaims.Concat(roleClaims);
        }

        public bool IsUserInRole(Guid userId, string role)
        {
            var user = _dbSet.Include(u => u.Roles).SingleOrDefault(u => u.UserId == userId);

            return user != null && user.Roles.Any(r => r.Name == role);
        }

        public User GetUser(Guid userId)
        {
            return _dbSet.Include(u => u.Roles)
                               .Include(u => u.Claims)
                               .SingleOrDefault(u => u.UserId == userId);
        }

        public User GetUser(string username)
        {
            return _dbSet.Include(u => u.Roles)
                            .Include(u => u.Claims)
                            .FirstOrDefault(u => u.UserName == username);
        }
    }
}
