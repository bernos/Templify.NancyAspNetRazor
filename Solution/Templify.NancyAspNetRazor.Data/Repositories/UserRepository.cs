using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data
{
    public class UserRepository  : IUserRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(DbContext context)
        {
            _context = context;
            _users = context.Set<User>();
        }

        public IEnumerable<string> GetClaimsForUser(Guid userId)
        {   
            var user = _users
                        .Include(u => u.Claims)
                        .Include(u => u.Roles)
                        .SingleOrDefault(u => u.UserId == userId);

            var userClaims = user.Claims.Select(c => c.Name).ToList();
            var roleClaims = user.Roles.SelectMany(r => r.Claims.Select(c => c.Name)).ToList();
                
            return user == null ? new string[0] : userClaims.Concat(roleClaims);
        }

        public bool IsUserInRole(Guid userId, string role)
        {
            var user = _users.Include(u => u.Roles).SingleOrDefault(u => u.UserId == userId);

            return user != null && user.Roles.Any(r => r.Name == role);
        }

        public User GetUser(Guid userId)
        {
                return _users.Include(u => u.Roles)
                               .Include(u => u.Claims)
                               .SingleOrDefault(u => u.UserId == userId);
        }

        public User GetUser(string username)
        {
            return _users.Include(u => u.Roles)
                            .Include(u => u.Claims)
                            .FirstOrDefault(u => u.UserName == username);
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
