using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Templify.NancyAspNetRazor.Data.Models
{
    public class User
    {
        public Guid UserId { get; private set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; private set; }
        public string Password { get; private set; }
        public virtual ICollection<Role> Roles { get; private set; }
        public virtual ICollection<Claim> Claims { get; private set; } 
        
        public User(string username, string password):this()
        {
            UserName = username;
            Password = password;
        }

        private User()
        {
            CreatedAt = DateTime.UtcNow;
            Roles = new Collection<Role>();
            Claims = new Collection<Claim>();
        }

        public User ChangePassword(string newPassword)
        {
            Password = newPassword;
            return this;
        }
    }
}
