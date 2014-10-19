using System;
using System.Collections.Generic;
using Bernos.DDD.Data;
using Templify.NancyAspNetRazor.Data.Auth.Models;

namespace Templify.NancyAspNetRazor.Data.Auth.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<string> GetClaimsForUser(Guid userId);
        bool IsUserInRole(Guid userId, string role);
        User GetUser(Guid userId);
        User GetUser(string username);
    }
}