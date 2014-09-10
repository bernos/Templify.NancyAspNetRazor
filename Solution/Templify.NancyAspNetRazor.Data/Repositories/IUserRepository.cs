using System;
using System.Collections;
using System.Collections.Generic;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data
{
    public interface IUserRepository
    {
        IEnumerable<string> GetClaimsForUser(Guid userId);
        bool IsUserInRole(Guid userId, string role);
        User GetUser(Guid userId);
        User GetUser(string username);

        void AddUser(User user);
        void Save();
    }
}